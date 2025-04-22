/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    f. Modificar la solución (e) para el caso en que sean 10 fotocopiadoras. El empleado le indica a la persona cuál fotocopiadora usar y cuándo hacerlo.
*/

Monitor Fotocopiadora{
    cond cola[N];
    cond emp;
    bool libre = true;
    cola fila;
    int auxId;

    Procedure siguiente(){
        if( not fila.empty() ){
            auxId = fila.pop();
            signal(cola[auxId]);
            libre = false;
        } else {
            libre = true;
        }
    }

    Procedure entrar(id: in int){
        // Persona entra y se agrega a la cola
        fila.push(id);
        wait(cola[id]);
    }

    Procedure salir(){
        // Persona sale y despierta al empleado
        signal(emp); 
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