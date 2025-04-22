/*
CHEQUEADO 

Simular la atención en una Terminal de Micros que posee 3 puestos para hisopar a 150 pasajeros. En cada puesto hay una Enfermera que atiende a los pasajeros de acuerdo con el orden de llegada al mismo. Cuando llega un pasajero se dirige al puesto que tenga menos gente esperando. Espera a que la enfermera correspondiente lo llame para hisoparlo. Cuando terminaron de hisoparlo se retira.
    a. Implemente una solución considerando que además de los pasajeros y las enfermeras hay un Coordinador que es quien le indica al pasajero cual es el puesto que tiene menos gente esperando (el puesto al cual debe ir).
    b. Implemente una solución en las que sólo existan los pasajeros y las enfermeras, siendo los pasajeros quienes determinan qué puesto tiene menos personas esperando.

Nota: suponga que existe una función Hisopar() que simula la atención del pasajero por parte de la enfermera correspondiente.
*/

int pasajeros = 150;
sem mutexPasajeros = 1;

cola fila[3];                // una fila por enfermera
sem mutexFila[] = ([3] = 1); // protege acceso a la fila

sem hayEnFila[] = ([3] = 0); // pasajeros en la fila

int pasajeroPorFila[] = ([3] = 0);
sem mutexPasajerosEnFila[] = ([3] = 1); // protege cantidad por fila

sem hisopado[] = ([150] = 0);

Process Pasajero[id: 0..149]
{
    int minFila = 0;

    // Buscar la fila con menos pasajeros
    for (int i = 0; i < 3; i++) {
        P(mutexPasajerosEnFila[i]);
        if (pasajeroPorFila[i] < pasajeroPorFila[minFila]) {
            minFila = i;
        }
        V(mutexPasajerosEnFila[i]);
    }

    // Agregarse a la fila seleccionada
    P(mutexFila[minFila]);
    fila[minFila].push(id);
    V(mutexFila[minFila]);

    P(mutexPasajerosEnFila[minFila]);
    pasajeroPorFila[minFila]++;
    V(mutexPasajerosEnFila[minFila]);

    V(hayEnFila[minFila]);  // Avisar a la enfermera que hay alguien en su fila
    P(hisopado[id]);        // Esperar ser hisopado
}

Process Enfermera[id: 0..2]
{
    int p;

    P(mutexPasajeros);
    while (pasajeros > 0) {
        pasajeros --;

        // Espera pasajero en su fila
        P(hayEnFila[id]);

        // Sacar pasajero de la fila
        P(mutexFila[id]);
        p = fila[id].pop();
        V(mutexFila[id]);

        // Simular hisopado
        Hisopar(p);

        // Liberar pasajero
        V(hisopado[p]);

        // Actualizar contador de personas en la fila
        P(mutexPasajerosEnFila[id]);
        pasajeroPorFila[id]--;
        V(mutexPasajerosEnFila[id]);

        // Control de finalización
        P(mutexPasajeros);
    }
    V(mutexPasajeros);
}
