/*
En una exposición aeronáutica hay un simulador de vuelo (que debe ser usado con
exclusión mutua) y un empleado encargado de administrar su uso. Hay P personas que esperan a que el empleado lo deje acceder al simulador, lo usa por un rato y se retira.
   
    b. Modifique la solución anterior para que el empleado además considere el orden de llegada para dar acceso al simulador.

Nota: cada persona usa sólo una vez el simulador.
*/

Process Persona[id; 0..P-1]{
    Empleado!llegada(id);

    Empleado?acceso();
    UsarSimulador(); // asumir que existe
    Empleado!liberar();
}

Process Fila{
    cola fila;
    int idP;
    while true{
        do Persona[*]?llegada(idP); --> 
            push(fila, idP);
        □ not empty(fila); Empleado?libre() -->
            pop(fila, idP);
            Empleado!siguiente(idP);     
    }
}

Process Empleado{

    while true{
        int idP;
        Fila!libre();
        Fila?siguiente(idP);
        Persona[idP]!acceso;
        // esperar a que termine de usar el simulador
        Persona[idP]?liberar();
    }
}
