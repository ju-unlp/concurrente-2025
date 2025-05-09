/*
Se debe simular una maratón con C corredores donde en la llegada hay UNA máquina
expendedoras de agua con capacidad para 20 botellas. Además, existe un repositor encargado de reponer las botellas de la máquina. Cuando los C corredores han llegado al inicio comienza la carrera. Cuando un corredor termina la carrera se dirigen a la máquina expendedora, espera su turno (respetando el orden de llegada), saca una botella y se retira. Si encuentra la máquina sin botellas, le avisa al repositor para que cargue nuevamente la máquina con 20 botellas; espera a que se haga la recarga; saca una botella y se retira. Nota: mientras se reponen las botellas se debe permitir que otros corredores se encolen.
*/

Process Corredor[id: 0.. C-1]{
    // llega a la carrera
    Carrera.llegaCorredor();
    // correr carrera
    Maquina.agarrarBotella();
}

Process Repositor{
    Maquina.cargarMaquna();
}

Monitor Carrera{
    cond corredores;
    int total = 0;

    procedure llegaCorredor(){
        // llega, se agrega al total
            // si estan todos empiezan a correr
            // sino me duermo hasta que esten todos
        total ++;
        if(total == C){
            signal_all(corredores);
        } else {
            wait(corredores);
        }
    }
}

Monitor Maquina{
    cond repositor;
    cond corredores;
    int enFila = 0;
    cond proximo;
    int esperandoMaquina = 0;
    

    int totalBotellas = 0;
    int stockBotellas = 20;

    bool reponiendo = false;

    procedure agarrarBotella(){
        enFila ++;
        // si hay gente en Fila
        if(enFila > 1){
            wait(corredores);
        }
        enFila --;
        // si no hay botellas
        if(stockBotellas == 0){
            if(not reponiendo){
                // despierto repositor
                signal(repositor);
                reponiendo = true;
            }
            // me duermo aparte
            esperandoMaquina ++;
            wait(proximo);
        } else {
            // saco botella
            stockBotellas --;
            totalBotellas ++;
            // si hay alguien esperando lo despierto
            if(esperandoMaquina > 0){
                signal(proximo);
            } else if (enFila > 0){
                signal(corredores);
            }
        }
    }

    procedure cargarMaquina(){
        // mientras hay botellas
        while(totalBotellas < C){
            // si hay stock me duermo
            if(stockBotellas > 0){
                wait(repositor);
            }

            stockBotellas = reponerBotellas();
            reponiendo = false;

            // despierto al proximo
            esperandoMaquina --;
            signal(proximo);

            // si todavia hay gente que no llego
            if(totalBotellas < C){
                wait(repositor);
            }
        }
    }

}