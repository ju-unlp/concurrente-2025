/*
Existen N personas que deben fotocopiar un documento. La fotocopiadora sólo puede ser usada por una persona a la vez. Analice el problema y defina qué procesos, recursos y monitores serán necesarios/convenientes, además de las posibles sincronizaciones requeridas para resolver el problema. Luego, resuelva considerando las siguientes situaciones:
    c. Modifique la solución de (a) para el caso en que se deba dar prioridad de acuerdo con la edad de cada persona (cuando la fotocopiadora está libre la debe usar la persona de mayor edad entre las que estén esperando para usarla).

*/

Monitor Fotocopiadora{
    cond cola[N];
    bool libre = true;
    int personas = 0;
    cola fila;
    int auxId;

    Procedure.entrar(id, edad: in int){
        if(not libre){
            fila.insertarOrdenado(id, edad); // inserta ordenado por edad mayor a menor
            wait(cola[id]);
            personas++;
        } else {
            libre = false;
        }
    }

    Procedure.salir(){
        if(personas > 0){
            auxId = fila.pop();
            personas--;
            signal(cola[auxId]);
        } else {
            libre = true;
        }
    }
}

Process Persona[id:0..N-1]{
    int edad = ...;
    Fotocopiadora.entrar(id, edad);
    // Fotocopiar
    Fotocopiadora.salir();
}