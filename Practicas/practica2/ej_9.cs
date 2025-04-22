/*
CHEQUEADO 

Resolver el funcionamiento en una fábrica de ventanas con 7 empleados (4 carpinteros, 1 vidriero y 2 armadores) que trabajan de la siguiente manera:
    • Los carpinteros continuamente hacen marcos (cada marco es armando por un único carpintero) y los deja en un depósito con capacidad de almacenar 30 marcos.
    • El vidriero continuamente hace vidrios y los deja en otro depósito con capacidad para 50 vidrios.
    • Los armadores continuamente toman un marco y un vidrio (en ese orden) de los depósitos correspondientes y arman la ventana (cada ventana es armada por un único armador)
*/

sem dMarco = 30; // capacidad del depósito de marcos
sem dVidrio = 50; // capacidad del depósito de vidrios
cola marcos, vidrios, ventanas;
sem mutexM = 1, mutexVid = 1, mutexVen = 1;
sem colaM = 0, colaV = 0; // cantidad de elementos encolados

Process Carpinteros[id: 0..3]
{
    marco m;
    while (true) {
        // hacer un marco
        P(mutexM); 
        P(dMarco);
        marcos.add(m);
        V(mutexM);
        V(colaM);
    }
}

Process Vidriero
{
    vidrio v;
    while (true) {
        // hacer un vidrio
        P(mutexV); 
        P(dVidrio);
        vidrios.add(v);
        V(mutexVid);
        V(colaV);
    }
}

Process Armadores[id: 0..1]
{
    marco m;
    vidrio v;
    ventana ven;
    while (true){
        P(colaM); // si hay marco encolado
        P(mutexM);
        m = marcos.remove();
        V(mutexM);
        v(dMarco); // libera deposito marco
        P(colaV); // si hay vidrio encolado
        P(mutexVid);
        v = vidrios.remove();
        V(mutexVid);
        V(dVidrio); // libera deposito vidrio
        // armar la ventana
        P(mutexVen);
        ventanas.add(ven);
        V(mutexVen);
    }
}