/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    f. Modificar la solución (e) para el caso en que sean 10 fotocopiadoras. El empleado le indica a la persona cuál fotocopiadora usar y cuándo hacerlo.
*/

Monitor Fotocopiadora {
    cond cola[N];
    cond emp;
    cola fila;
    int auxId;
    cola fotocLibres;

    Procedure siguiente() {
        if (not fila.empty() and not fotocLibres.empty()) {
            auxId = fila.pop();
            signal(cola[auxId]);
        }
        wait(emp);
    }

    Procedure entrar(id: in int, fotocId: out int) {
        fila.push(id);
        signal(emp); 
        wait(cola[id]); 
        /* asignar fotocopiadora */
        fotocId = fotocLibres.pop();
    }

    Procedure salir(fotocId: in int) {
        fotocLibres.push(fotocId);
        signal(emp); 
    }
}

Process Persona[id:0..N-1]{
    int fotocId = -1;

    Fotocopiadora.entrar(id, fotocId);
    // Fotocopiar[fotocId]();
    Fotocopiadora.salir(fotocId);
}

Process Empleado{
    while(true){
        Fotocopiadora.siguiente();
    }
}