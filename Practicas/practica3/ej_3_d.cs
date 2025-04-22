/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    d. Modifique la solución de (a) para el caso en que se deba respetar estrictamente el orden dado por el identificador del proceso (la persona X no puede usar la fotocopiadora hasta que no haya terminado de usarla la persona X-1).
*/

Monitor Fotocopiadora{
    cond cola;
    int idActual = 0;

    Procedure.entrar(id: in int){
        if(idActual <> id){
            wait(cola[id]);
        }
    }

    Procedure.salir(){
        idActual++;
        signal_all(cola);
    }
}

Process Persona[id:0..N-1]{
    Fotocopiadora.entrar(id);
    // Fotocopiar
    Fotocopiadora.salir();
}