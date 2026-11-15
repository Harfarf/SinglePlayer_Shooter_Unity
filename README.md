# Single Player Shooter – Unity

## Descripción

Experiencia interactiva de tipo *Single Player Shooter* desarrollada en Unity y C#. El desarrollo se centra en la implementación de mecánicas de juego, la organización del código y el uso de diferentes técnicas de programación con C#.

## 1. Programación orientada a objetos

Se aplican los cuatro principios fundamentales:

- **Abstracción:** mediante clases, que agrupan datos y comportamientos.
- **Encapsulación:** a través de modificadores como `private` y `public` para controlar el acceso a variables y métodos de las clases.
- **Herencia:** se utiliza mediante clases que heredan de `MonoBehaviour` y `ScriptableObject`, aprovechando las funcionalidades que proporciona Unity.
- **Polimorfismo:** mediante interfaces como `IDamageable`, implementada tanto por el jugador como por los enemigos. Esto permite utilizar métodos comunes, como `SetDamage()`, sin depender de la clase concreta del objeto.

## 2. API de Unity y ciclo de vida

Se utilizan funciones propias de la API de Unity, como `Awake`, `Start`, `Update` y `LateUpdate`, para gestionar el ciclo de vida de los componentes. También se usan sus sistemas de física y detección, como colisiones, triggers y raycasts, para gestionar las interacciones entre objetos.

### Control del jugador y cámara

El control del jugador utiliza el **New Input System** de Unity, centralizado mediante `InputManager`. El movimiento se gestiona con `Rigidbody`, mientras que un `Raycast` permite comprobar si el jugador está en contacto con el suelo antes de saltar.

La cámara utiliza `LateUpdate()` para seguir al jugador después de actualizar su movimiento, manteniendo una posición relativa y permitiendo controlar su rotación.

## 3. Patrones de diseño y sistemas de juego

### Singleton y Managers

Se utiliza el patrón Singleton en `DataManager`, `InputManager`, `ObjectPool` y `LoadSceneHandler`, garantizando una única instancia de cada sistema y facilitando su acceso desde otras partes del proyecto.

- **DataManager:** se encarga del guardado y la carga de las estadísticas del jugador mediante archivos JSON. Utiliza `JsonUtility` para serializar y deserializar los datos y `System.IO` para escribirlos y leerlos.
- **InputManager:** centraliza las acciones del New Input System, proporcionando métodos para consultar botones y ejes de movimiento y cámara.
- **LoadSceneHandler:** gestiona los cambios de escena mediante corrutinas y carga asíncrona. Utiliza una escena intermedia de carga que se muestra mientras descarga la escena anterior y carga la siguiente.
- **ObjectPool:** mediante queues y callbacks centraliza la creación y gestión de los pools de objetos y efectos visuales que se reutilizarán durante la partida.

### Object Pooling

Se implementa una pool para reutilizar objetos en runtime, más específicamente las balas disparadas por el jugador y los enemigos generados, así como las dinamitas y efectos visuales.

El método `CreatePool()` genera un conjunto de objetos a partir de un prefab, los desactiva y los añade a una queue (`Queue<GameObject>`). Además, permite ejecutar acciones opcionales (`Action<GameObject>`) durante su inicialización.

Para los efectos visuales se utiliza una corrutina que obtiene un objeto de la queue, lo posiciona, activa y reproduce su `ParticleSystem`. Cuando termina la reproducción, el objeto se desactiva y vuelve a la queue.

**Ejemplo:** el sistema de balas utiliza una pool de 20 proyectiles gestionada por `PlayerAttack`. Al disparar, se obtiene una bala disponible, se configura su posición y dirección y se mueve mediante una corrutina que actualiza la velocidad de su `Rigidbody`. Cuando la bala impacta o termina su tiempo de vida, se desactiva y vuelve a la queue.

### Gestión y respawn de enemigos

El script `EnemySpawner` gestiona la creación y reutilización de enemigos y dinamitas mediante pools. Al inicio de la partida genera queues para ambos y los posiciona aleatoriamente sobre el NavMesh alrededor del jugador.

Mediante eventos (`Action`), el spawner recibe una notificación cuando un enemigo muere, lo devuelve a la queue y solicita su reaparición. Las dinamitas también vuelven a su pool cuando se desactivan.

### Máquina de estados de los enemigos

El `EnemyController` implementa una máquina de estados con dos comportamientos: `Patrol` y `Chase`. Según la distancia al jugador, el enemigo patrulla por posiciones aleatorias del NavMesh o lo persigue utilizando `NavMeshAgent`. Si alcanza al jugador, ejecuta su método `OnDie()` a través de la interfaz `IDamageable`.

### Sistema de dinamita

El `DynamiteController` gestiona la cuenta atrás y explosión de las dinamitas. Antes de explotar, utiliza DOTween para animar el color del material y avisar visualmente al jugador. Al finalizar, busca los objetos que implementan `IDamageable` y aplica el efecto de la explosión tanto al jugador como a los enemigos dentro del radio establecido.

Finalmente, reproduce un efecto visual y notifica mediante un evento al spawner para devolver la dinamita a su pool.

### Eventos y gestión de datos

Se utilizan eventos (`Actions`) para comunicar cambios entre sistemas, como la muerte de enemigos, la devolución de objetos a sus pools y la actualización de las estadísticas del jugador.

El `PlayerController` registra las bajas y muertes del jugador y utiliza `DataManager` para guardar y recuperar estos datos, manteniendo las estadísticas entre partidas.

Por su parte, `UIController` se suscribe a los eventos del jugador para actualizar los contadores de bajas y muertes mediante TextMeshPro y mostrar la pantalla de derrota cuando el jugador muere. También gestiona la pausa del juego mediante `Time.timeScale`.

## 4. Programación asíncrona

El script `Bullet` utiliza `async/await` junto con `Awaitable.WaitForSecondsAsync` para controlar el tiempo de vida de los proyectiles sin bloquear la ejecución del juego. También emplea `destroyCancellationToken` para cancelar la espera si el objeto es destruido.

Se utiliza `try-catch` para gestionar la excepción `OperationCanceledException` cuando se cancela la operación asíncrona.

## 5. ScriptableObjects

Se utiliza un `ScriptableObject` llamado `Enemy_SO` para almacenar configuraciones de los enemigos, como su color y salud máxima.

El `EnemyController` dispone de una lista de estas configuraciones y selecciona una aleatoriamente al inicializarse. De esta forma, se pueden crear diferentes variantes de enemigos modificando sus datos desde el Inspector, sin necesidad de crear clases o scripts distintos para cada variante.

## 6. Generación procedural

El terreno se genera proceduralmente mediante **Perlin Noise**, construyendo una malla (*Mesh*) a partir de las alturas calculadas. Se utilizan dos materiales diferentes, asignando hierba o roca según la altura de cada zona del terreno.

## Tecnologías y conceptos

- Unity y C#
- Programación orientada a objetos e interfaces
- New Input System
- Rigidbody, Raycast y NavMesh
- Singleton
- JSON y `System.IO`
- Carga asíncrona de escenas
- Object Pooling y colecciones `Queue`
- Eventos y delegados `Action`
- Corrutinas y `async/await`
- Gestión de excepciones con try-catch
- ScriptableObjects
- Máquinas de estados
- DOTween
- TextMeshPro
- Generación procedural con Perlin Noise y Meshes
