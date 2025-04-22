/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    a. Implemente una solución suponiendo no importa el orden de uso. Existe una función Fotocopiar() que simula el uso de la fotocopiadora.
*/

Monitor Fotocopiadora{
    cond cola;
    bool libre = true;
    int personas = 0;

    Procedure.entrar(){
        if(not libre){
            personas ++;
            wait(cola);
        } else {
            libre = false;
        }
    }

    Procedure.salir(){
        if(personas > 0){
            personas --;
            signal(cola);
        } else {
            libre = true;
        }
    }
}

Process Persona[id:0..N-1]{
    Fotocopiadora.entrar();
    // Fotocopiar
    Fotocopiadora.salir();
}