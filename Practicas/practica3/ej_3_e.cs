/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    e. Modifique la solución de (b) para el caso en que además haya un Empleado que le indica a cada persona cuando debe usar la fotocopiadora.
*/

Monitor Fotocopiadora {
    cond cola[N];
    cond emp;
    cola fila;
    int auxId;
    bool libre = true;

    Procedure siguiente() {
        if (not fila.empty() and libre) {
            libre = false;
            auxId = fila.pop();
            signal(cola[auxId]);
        }
        wait(emp);
    }

    Procedure entrar(id: in int) {
        fila.push(id);
        signal(emp); // despierta al empleado
        wait(cola[id]); // espera a que lo habiliten
    }

    Procedure salir() {
        libre = true;
        signal(emp); // avisa al empleado que terminó
    }
}

Process Persona[id:0..N-1]{
    Fotocopiadora.entrar(id);
    // Fotocopiar
    Fotocopiadora.salir();
}

Process Empleado{
    while(true){
        Fotocopiadora.siguiente();
    }
}