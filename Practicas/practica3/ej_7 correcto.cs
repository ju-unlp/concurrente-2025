/*
Se debe simular una maratón con C corredores donde en la llegada hay UNA máquina
expendedoras de agua con capacidad para 20 botellas. Además, existe un repositor encargado de reponer las botellas de la máquina. Cuando los C corredores han llegado al inicio comienza la carrera. Cuando un corredor termina la carrera se dirigen a la máquina expendedora, espera su turno (respetando el orden de llegada), saca una botella y se retira. Si encuentra la máquina sin botellas, le avisa al repositor para que cargue nuevamente la máquina con 20 botellas; espera a que se haga la recarga; saca una botella y se retira. Nota: mientras se reponen las botellas se debe permitir que otros corredores se encolen.
*/

Process Corredor[id: 0.. C-1]{
    Carrera.llegaCorredor();
    Maquina.solicitarBotella();
    Maquina.agarrarBotella();
}

Process Repositor{
    bool continuar = true;
    Maquina.esperarReposicion(continuar);
    while(continuar){
        for (int i = 1 to 20) {
            Maquina.reponerBotella();
        }
        Maquina.esperarReposicion(continuar);
    }
}

Monitor Carrera{
    cond corredores;
    int total = 0;
    procedure llegaCorredor(){
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
    bool reponiendo = true;
    int totalBotellas = 0;
    int stockBotellas = 0;

    cond corredores;
    int esperandoEnFila = 0;
    cond proximo;
    bool esperandoBotella = false;

    procedure esperarReposicion(continuar: out bool){
        if(totalBotellas < C){
            continuar = true;
            if(stockBotellas > 0){
                wait(repositor);
            }
        } else {
            continuar = false;
        }
    }   

    procedure reponerBotella(){
        stockBotellas ++;
        totalBotellas ++;
        if(stockBotellas == 20){
            reponiendo = false;
            if(esperandoBotella){
                esperandoBotella = false;
                signal(proximo);
            }
        }
    }

    procedure solicitarBotella(){
        esperandoEnFila ++;
        if(esperandoEnFila > 1 or reponiendo){
            wait(corredores);
        }
    }

    procedure agarrarBotella(){
        // si no hay botellas llamo repositor
        if(stockBotellas == 0){
            reponiendo = true;
            signal(repositor);
            esperandoBotella = true;
            wait(proximo); // me duermo aparte
        }
        stockBotellas --;
        esperandoEnFila--;
        if(esperandoEnFila > 0){
            signal(corredores);
        }
    }
}