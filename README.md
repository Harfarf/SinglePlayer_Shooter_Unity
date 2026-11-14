# Single Player Shooter – Unity

## Descripción

Proyecto de una experiencia interactiva *Single Player Shooter* desarrollado en Unity y C#. El desarrollo se centra en la implementación de las mecánicas del juego, la organización del código y el uso de diferentes técnicas de programación.

## 1. Programación orientada a objetos

Durante el desarrollo se aplican los cuatro principios fundamentales de la programación orientada a objetos:

- **Abstracción:** las clases representan los distintos elementos y sistemas del juego, agrupando sus datos y comportamientos.
- **Encapsulación:** se utilizan modificadores como `private` y `public` para controlar el acceso al estado y los métodos de cada clase.
- **Herencia:** se aprovecha la estructura de `MonoBehaviour` de Unity y la herencia de C# para reutilizar y extender comportamientos.
- **Polimorfismo:** se utilizan interfaces como `IDamageable`, implementada tanto por el jugador como por los enemigos. Esto permite interactuar con ambos mediante los mismos métodos, independientemente de su implementación concreta. También se utiliza en el sistema de proyectiles para aplicar daño a cualquier objeto que implemente dicha interfaz.

## 2. API de Unity y ciclo de vida

Se utiliza la arquitectura de componentes de Unity mediante `MonoBehaviour`, junto con métodos del ciclo de vida como `Awake`, `Start`, `Update` y `LateUpdate`. También se emplean colisiones, triggers y raycasts para gestionar las interacciones entre objetos.

### Control del jugador y cámara

El control del jugador utiliza el **New Input System** de Unity, centralizado mediante `InputManager`. El movimiento se gestiona con `Rigidbody`, mientras que un `Raycast` permite comprobar si el jugador está en contacto con el suelo antes de saltar.

La cámara utiliza `LateUpdate()` para seguir al jugador después de actualizar su movimiento, manteniendo una posición relativa y permitiendo controlar su rotación.

## 3. Patrones de diseño y sistemas de juego

### Singleton y managers

Se utiliza el patrón Singleton en `DataManager`, `InputManager`, `ObjectPool` y `LoadSceneHandler`, proporcionando acceso centralizado a estos sistemas.

- **DataManager:** gestiona el guardado y la carga de las estadísticas del jugador mediante archivos JSON. Utiliza `JsonUtility` para serializar y deserializar los datos y `System.IO` para escribirlos y leerlos.
- **InputManager:** centraliza las acciones del New Input System, proporcionando métodos para consultar botones y ejes de movimiento y cámara. También permite habilitar o deshabilitar la entrada.
- **LoadSceneHandler:** gestiona los cambios de escena mediante corrutinas y carga asíncrona. Utiliza una escena intermedia de carga que se muestra mientras descarga la escena anterior y carga la siguiente.
- **ObjectPool:** centraliza la creación y gestión de los pools de objetos y efectos visuales, utilizando colas y callbacks para configurar las instancias.

### Object Pooling

Se implementa un sistema de Object Pooling para reutilizar objetos durante la ejecución, especialmente balas disparadas por el jugador y los enemigos, así como dinamitas y efectos visuales.

El método `CreatePool()` genera un conjunto de objetos a partir de un prefab, los desactiva y los añade a una cola (`Queue<GameObject>`). Además, permite ejecutar acciones opcionales (`Action<GameObject>`) durante su inicialización.

Para los efectos visuales se utiliza una corrutina que obtiene un objeto de la cola, lo posiciona, activa y reproduce su `ParticleSystem`. Cuando termina la reproducción, el objeto se desactiva y vuelve a la cola.

El sistema de balas utiliza una cola de 20 proyectiles gestionada por `PlayerAttack`. Al disparar, se obtiene una bala disponible, se configura su posición y dirección y se mueve mediante una corrutina que actualiza la velocidad de su `Rigidbody`. Cuando la bala impacta o termina su tiempo de vida, se desactiva y vuelve a la cola.

### Gestión y respawn de enemigos

El script `EnemySpawner` gestiona la creación y reutilización de enemigos y dinamitas mediante pools. Inicialmente prepara una cola de 8 enemigos y otra de 5 dinamitas. Los enemigos se posicionan aleatoriamente sobre el NavMesh alrededor del jugador.

Mediante eventos (`Action`), el spawner recibe la notificación cuando un enemigo muere, lo devuelve a la cola y solicita su reaparición. Las dinamitas también vuelven a su pool cuando se desactivan.

### Máquina de estados de los enemigos

El `EnemyController` implementa una máquina de estados sencilla con dos comportamientos: `Patrol` y `Chase`. Según la distancia al jugador, el enemigo patrulla por posiciones aleatorias del NavMesh o lo persigue utilizando `NavMeshAgent`. Si alcanza al jugador, ejecuta su método `OnDie()` a través de la interfaz `IDamageable`.

### Sistema de dinamita

El `DynamiteController` gestiona la cuenta atrás y explosión de las dinamitas. Antes de explotar, utiliza DOTween para animar el color del material y avisar visualmente al jugador. Al finalizar, busca los objetos que implementan `IDamageable` y aplica el efecto de la explosión tanto al jugador como a los enemigos dentro del radio establecido.

Finalmente, reproduce un efecto visual y notifica mediante un evento al spawner para devolver la dinamita a su pool.

### Eventos y gestión de datos

Se utilizan eventos (`Action`) para comunicar cambios entre sistemas, como la muerte de enemigos, la devolución de objetos a sus pools y la actualización de las estadísticas del jugador.

El `PlayerController` registra las bajas y muertes del jugador y utiliza `DataManager` para guardar y recuperar estos datos, manteniendo las estadísticas entre partidas.

Por su parte, `UIController` se suscribe a los eventos del jugador para actualizar los contadores de bajas y muertes mediante TextMeshPro y mostrar la pantalla de derrota cuando el jugador muere. También gestiona la pausa del juego mediante `Time.timeScale`.

## 4. Programación asíncrona

El script `Bullet` utiliza `async/await` junto con `Awaitable.WaitForSecondsAsync` para controlar el tiempo de vida de los proyectiles sin bloquear la ejecución del juego. También emplea `destroyCancellationToken` para cancelar la espera si el objeto es destruido.

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
- ScriptableObjects
- Máquinas de estados
- DOTween
- TextMeshPro
- Generación procedural con Perlin Noise y Meshes
