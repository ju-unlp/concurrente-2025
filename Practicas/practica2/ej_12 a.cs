/*
Simular la atención en una Terminal de Micros que posee 3 puestos para hisopar a 150 pasajeros. En cada puesto hay una Enfermera que atiende a los pasajeros de acuerdo con el orden de llegada al mismo. Cuando llega un pasajero se dirige al puesto que tenga menos gente esperando. Espera a que la enfermera correspondiente lo llame para hisoparlo. Cuando terminaron de hisoparlo se retira.
    a. Implemente una solución considerando que además de los pasajeros y las enfermeras hay un Coordinador que es quien le indica al pasajero cual es el puesto que tiene menos gente esperando (el puesto al cual debe ir).
    b. Implemente una solución en las que sólo existan los pasajeros y las enfermeras, siendo los pasajeros quienes determinan qué puesto tiene menos personas esperando.

Nota: suponga que existe una función Hisopar() que simula la atención del pasajero por parte de la enfermera correspondiente.
*/

cola pasajeros;
sem mutexPasajeros = 1; 

cola fila[3]; // una fila por enfermera
sem mutexFila[] = ([3] = 1); // mutex para fila

sem hayEnFila[] = ([3] = 0); // hay alguien en la fila?
int pasajeroPorFila[] = ([3] = 0); // cantidad de pasajeros por fila
sem mutexPasajerosEnFila[] = ([3] = 1); // mutex para pasajeros por fila

sem hisopado[] = ([150] = 0);



Process Pasajero[id: 0..149]
{
    P(mutexPasajeros);
    pasajeros.push(id); // se agrega a la fila general
    V(mutexPasajeros);

    P(hisopado[id]);
}

Process Enfermera[id: 0..2]
{
    pasajero p;
    while (true) {
        P(hayEnFila[id]); // si hay gente en su fila
        P(mutexFila[id]);
        p = fila[id].pop(); // lo saca de la fila
        V(mutexFila[id]);

        Hisopar(p); // lo hisopa
        V(hisopado[p]); // lo marca como hisopado para que se pueda ir

        P(mutexPasajerosEnFila[id]);
        pasajeroPorFila[id]--; // decrementa la cantidad de gente en mi fila
        V(mutexPasajerosEnFila[id]);
    }
}

Process Coordinador
{
    pasajero p;
    while (true) {
        P(mutexPasajeros);
        if (not pasajeros.empty()) {
            p = pasajeros.pop(); // se saca un pasajero de la cola general
        }
        V(mutexPasajeros);

        // Calcula fila con menos pasajeros
        int minFila = 0;
        for (int i = 0; i < 3; i++) {
            P(mutexFila[i]);
            if (pasajeroPorFila[i] < pasajeroPorFila[minFila]) {
                minFila = i;
            }
            V(mutexFila[i]);
        }

        // agrega el pasajero a la fila con menos pasajeros
        P(mutexFila[minFila]);
        fila[minFila].push(p); 
        V(mutexFila[minFila]);

        P(mutexPasajerosEnFila[minFila]);
        pasajeroPorFila[minFila]++; // aumenta la cantidad de gente en la fila
        V(mutexPasajerosEnFila[minFila]);
    }
}