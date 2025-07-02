/*
Resolver con SEMÁFOROS. En una estación de trenes, asisten P personas que deben realizar una carga de su tarjeta SUBE en la terminal disponible. La terminal es utilizada en forma exclusiva por cada persona de acuerdo con el orden de llegada. Implemente una solución utilizando sólo emplee procesos Persona. Nota: la función UsarTerminal() le permite cargar la SUBE en la terminal disponible.
*/

sem mutex = 1;
bool libre = true;
queue fila;
sem turno[P] = ([P]0);

Process Persona[id: 0.. P-1]{
    int auxId;
    P(mutex);
    if(libre){
        libre = false;
        V(mutex);
    } else {
        push(fila, id);
        V(mutex);
        P(turno[id]);
    }

    UsarTerminal();

    P(mutex);
    if(not empty(fila)){
        pop(fila, auxId);
        V(turno[auxId]);
    } else {
        libre = true;
    }
    V(mutex);
}