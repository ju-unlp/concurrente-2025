/*
CHEQUEADO 

En un vacunatorio hay un empleado de salud para vacunar a 50 personas. El empleado de salud atiende a las personas de acuerdo con el orden de llegada y de a 5 personas a la vez. Es decir, que cuando está libre debe esperar a que haya al menos 5 personas esperando, luego vacuna a las 5 primeras personas, y al terminar las deja ir para esperar por otras 5. Cuando ha atendido a las 50 personas el empleado de salud se retira. 

Nota: todos los procesos deben terminar su ejecución; asegurarse de no realizar Busy Waiting; suponga que el empleado tienen una función VacunarPersona() que simula que el empleado está vacunando a UNA persona.
*/

int cantPersonas = 50;
cola personas;
sem mutexPersonas = 1;
sem turno[] = ([50] = 0);
sem esperandoTurno = 0;
bool vacunado[] = ([50] = false);

Process Persona[id: 1..49]
{
    if not vacunado[id] { // condicion de salida
        P(mutexPersonas);
        personas.add(id); // se unen a la cola
        V(mutexPersonas);

        V(esperandoTurno); // avisan que estan esperando

        P(turno[id]); // esperan que las vacunen
        vacunado[id] = true; // se marcan como vacunadas
    }
}

Process Empleado
{
    persona p;
    for( int i = 0; i < cantPersonas; i+=5 ) {
        for(int i = 0; i < 5; i++) {
            P(esperandoTurno); // espero a que haya 5 personas
        }

        for(int i = 0; i < 5; i++) {
            P(mutexPersonas); // me aseguro de que no haya nadie en la cola
            p = personas.remove();
            V(mutexPersonas);
            VacunarPersona(p); // vacuno a la persona
            V(turno[p]); // le aviso que ya esta vacunada
        }
    }
}
