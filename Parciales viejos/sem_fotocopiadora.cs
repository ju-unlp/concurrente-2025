/// CORREGIDO ///
/* 
Resolver con SEMÁFOROS el siguiente problema. Simular el uso de un conjunto de 10 fotocopiadoras en una empresa. Hay un empleado que se encarga de atender a los N usuarios que llegan para usar una fotocopiadora, de acuerdo al orden de llegada. Cada usuario al llegar espera hasta que el empleado le indica a que fotocopiadora ir, la usan y se retira. 
Nota: maximizar la concurrencia; suponga que hay una función Fotocopiar() llamada por el usuario que simula el uso de la fotocopiadora.
*/

// Asumir objeto Fotocopiadora que simula su uso

sem mutexUsuarios = 1;
sem hayUsuario = 0;
sem mutexFotoc = 1;
sem turno[N] = ([N] 0);
sem fotocopiadoras = 10;
queue colaUsuarios, colaFotocopiadoras;
int numFotoc[N];

Process Usuario[id: 0.. N-1]{
    // Me agrego a la fila y espero mi turno
    P(mutexUsuarios);
    colaUsuarios.push(id);
    V(mutexUsuarios);

    V(hayUsuario);

    P(turno[id]);
    // agarro mi fotocopiadora, la uso, la libero
    int f = numFotoc[id];
    Fotocopiar(f);


    P(mutexFotoc);
    colaFotocopiadoras.push(f);
    V(mutexFotoc);

    V(fotocopiadoras);
}

Process Empleado{
    for(int i = 1 to N){
        P(fotocopiadoras);
        P(hayUsuario);
        
        P(mutexUsuario);
        int id = colaUsuarios.pop();
        V(mutexUsuario);

        P(mutexFotoc);
        fotocUsuario[id] = colaFotocopiadoras.pop();
        v(colaFotoc);

        V(turno[id]);
    }
}