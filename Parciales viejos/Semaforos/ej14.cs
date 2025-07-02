/*
Resolver con SEMÁFOROS el siguiente problema. En una fábrica de muebles trabajan 50 empleados. A llegar, los empleados forman 10 grupos de 5 personas cada uno, de acuerdo al orden de llegada (los 5 primeros en llegar forman el primer grupo, los 5 siguientes el segundo grupo, y así sucesivamente). Cuando un grupo se ha terminado de formar, todos sus integrantes se ponen a trabajar. Cada grupo debe armar M muebles (cada mueble es armado por un solo empleado); mientras haya muebles por armar en el grupo los empleados los irán resolviendo (cada mueble es armado por un solo empleado). Nota: Cada empleado puede tardar distinto tiempo en armar un mueble. Sólo se pueden usar los procesos “Empleado”, y todos deben terminar su ejecución. Maximizar la concurrencia.
*/

sem mutex = 1;
int nroGrupo = 1;
int empleados = 0;
sem empezar[5] = ([5] = 0);
int mueblesPorArmar[5] = ([5] = M);
sem mutexGrupo[5] = ([5] = 1);

Process Empleado[id: 0..49]{
    int nro;
    mueble m;
    // llega y se le asigna grupo
    P(mutex);
    nro = nroGrupo;
    empleados ++; 
    if(empleados == 5){
        for (int i = 1 to 5){
            V(empezar[nroGrupo]);
        }
        nroGrupo ++;
        empleados = 0;
    }
    V(mutex);
    // espera que se termine de formar grupo
    P(empezar[nro]);
    // armar muebles hasta llegar a M
    P(mutexGrupo[nro]);
    while(mueblesPorArmar[nro] > 0){
        mueblesPorArmar[nro] --;
        V(mutexGrupo[nro]);
        armarMueble(m);
        P(mutexGrupo[nro]);
    }
    V(mutexGrupo[nro]);
}