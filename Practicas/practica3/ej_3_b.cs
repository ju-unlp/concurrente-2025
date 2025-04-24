/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    b. Modifique la solución de (a) para el caso en que se deba respetar el orden de llegada.
*/

Monitor Fotocopiadora{
    cond cola;
    bool libre = true;
    int personas = 0;

    Procedure.entrar(id: in int){
        if(not libre){
            wait(cola);
            personas++;
        } else {
            libre = false;
        }
    }

    Procedure.salir(){
        if(personas > 0){
            personas--;
            signal(cola);
        } else {
            libre = true;
        }
    }
}

Process Persona[id:0..N-1]{
    Fotocopiadora.entrar(id);
    // Fotocopiar
    Fotocopiadora.salir();
}