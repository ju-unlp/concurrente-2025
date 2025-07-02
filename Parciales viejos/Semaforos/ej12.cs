/*
En una estación de trenes, asisten P personas que deben realizar una carga de su tarjeta SUBE en la terminal disponible. La terminal es utilizada en forma exclusiva por cada persona de acuerdo con el orden de llegada. Implemente una solución utilizando sólo emplee procesos Persona. Nota: la función UsarTerminal() le permite cargar la SUBE en la terminal disponible.

Resuelva el mismo problema anterior pero ahora considerando que hay T terminales disponibles. Las personas realizan una única fila y la carga la realizan en la primera terminal que se libera. Recuerde que sólo debe emplear procesos Persona. Nota: la función UsarTerminal(t) le permite cargar la SUBE en la terminal t.
*/

sem mutex = 1;
bool libre = true;
queue fila;
sem turno[P] = ([P]0);
sem terminalesLibres = T;


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

    P(terminalesLibres);
    UsarTerminal(t);
    V(terminalesLibres);

    P(mutex);
    if(not empty(fila)){
        pop(fila, auxId);
        V(turno[auxId]);
    } else {
        libre = true;
    }
    V(mutex);
}