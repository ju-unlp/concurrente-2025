/*
Existen N procesos que deben leer información de una base de datos, la cual es administrada por un motor que admite una cantidad limitada de consultas simultáneas.
    a. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema.
    b. Implemente el acceso a la base por parte de los procesos, sabiendo que el motor de base de datos puede atender a lo sumo 5 consultas de lectura simultáneas.
*/

Monitor BaseDeDatos {
    cond cola;
    int consultas = 0;
    int esperando = 0;

    Procedure entrar(){
        if(consultas == 5){
            esperando ++;
            wait(cola);
        } else {
            consultas++;
        }
    }

    Procedure salir(){
        if( esperando > 0){
            esperando--;
            signal(cola);
        } else {
            consultas--;
        }
    }
}

Process P [id: 0..N-1]{
    BaseDeDatos.entrar();
    //usa la base de datos
    BaseDeDatos.salir();
}