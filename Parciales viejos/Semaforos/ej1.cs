/*
Resolver con SEMÁFOROS el siguiente problema. Simular el uso de un conjunto de 10 fotocopiadoras en una empresa. Hay un empleado que se encarga de atender a los N usuarios que llegan para usar una fotocopiadora, de acuerdo al orden de llegada. Cada usuario al llegar espera hasta que el empleado le indica a que fotocopiadora ir, la usan y se retira. 
Nota: maximizar la concurrencia; suponga que hay una función Fotocopiar() llamada por el usuario que simula el uso de la fotocopiadora.
*/

queue fotocopiadoras = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
queue fila;
int fotocopiadora[N] = ([N]0);
sem mutexFila, mutexF = 1;
sem imprimir[N] = ([N]0);
sem esperando = 0;
sem libres = 10;

Process Empleado{
    int auxU, auxF;
    
    while true{
        // si hay fotocopiadora libre
        P(libres);
        // si hay alguien esperando
        P(esperando);
        // agarro el id
        P(mutexFila);
        pop(fila, auxU);
        V(mutexFila);
        // le asigno fotocopiadora
        P(mutexF);
        pop(fotocopiadoras, auxF);
        fotocopiadora[auxU] = auxF;
        V(mutexF);
        // le doy el paso
        V(imprimir[auxU]);
    }
}

Process Usuario[id: 1..N]{
    int auxF;
    // me agrego al final de la cola
    P(mutexFila);
    push(fila,id);
    V(mutexFila);
    // aviso que llegue
    V(esperando);
    // espero mi turno
    P(imprimir[id]);
    // saco la impresora asignada
    P(mutexF);
    pop(fotocopiadoras, auxF);
    V(mutexF);
    // fotocopiar
    Fotocopiar();
    // devolver fotocopiadora
    P(mutexF);
    push(fotocopiadoras, auxF);
    V(mutexF);
    V(libres);
}