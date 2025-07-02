/*
Resolver el siguiente problema con MONITORES. En una montaña hay 30 escaladores que en una parte de la subida deben utilizar un único paso de a uno a la vez y de acuerdo al orden de llegada al mismo. Nota: sólo se pueden utilizar procesos que representen a los escaladores; cada escalador usa sólo una vez el paso.
*/

Monitor Montaña{
    cond escaladores;
    bool pasando = false;
    int cantFila;

    Procedure llegada(){
        if(pasando){
            cantFila ++;
            wait(escaladores);
        } else {
            pasando = true;
        }
    }

    Procedure siguiente(){
        if(cantFila > 0){
            cantFila --;
            signal(escaladores);
        } else {
            pasando = false;
        }
    }
}

Process Escalador[id: 0..29]{
    
    Montaña.llegada();
    pasarSubida();
    Montaña.siguiente();
}