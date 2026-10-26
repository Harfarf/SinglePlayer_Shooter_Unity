using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class test : MonoBehaviour
{
    List<int> myList = new List<int> { 16859, 23, 5432, 5435, 7, 7, 7, 33, 49, 58, 345, 675, 84883, 4956 };


    private void Start()
    {
        myList.Where(x => x % 2 == 1).OrderBy(x => +x).Distinct().ToList().ForEach(x => Debug.Log(x));


        //esto no modifica la colcción original te da una nueva colección con los elementos que cumplen la condición
        //where: odd numbers
        //order by: ascending order (if + is ascending, for descending is -)
        //distinct: remove duplicates
        //toList: convert to list
        //forEach: iterate through the list and print each element
    }









    //generar explosivos objectpool - en el area around th player - que exploten a los pocos segundos de apareecer y mata al player. raycast para ver si está en rango???? si no hay obstacles


    //saltar si estas pisando el suelo
    //guardad en json las kills for example
    //Los personajes enemigos que irán aumentando su tamaño progresivamente, además del daño de sus disparos
    //
    //scriptable objetcs for some enemy data

    //navmesh and navmeshobstacles






    //

}
