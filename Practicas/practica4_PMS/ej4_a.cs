/*
En una exposición aeronáutica hay un simulador de vuelo (que debe ser usado con
exclusión mutua) y un empleado encargado de administrar su uso. Hay P personas que esperan a que el empleado lo deje acceder al simulador, lo usa por un rato y se retira.
    a. Implemente una solución donde el empleado sólo se ocupa de garantizar que el simulador es usado por una persona a la vez.
    
Nota: cada persona usa sólo una vez el simulador.
*/

Process Persona[id; 0..P-1]{
    Empleado!llegada(id);

    Empleado?acceso();
    UsarSimulador(); // asumir que existe
    Empleado!liberar();
}

Process Empleado{

    while true{
        Persona[*]?llegada(idP);
        Persona[idP]!acceso();
        // esperar a que termine de usar el simulador
        Persona[idP]?liberar();
    }
}
