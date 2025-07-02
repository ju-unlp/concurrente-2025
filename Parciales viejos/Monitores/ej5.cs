/*
Resolver con MONITORES el siguiente problema. En una planta verificadora de vehículos existen 5 estaciones de verificación. Hay 75 vehículos que van para ser verificados, cada uno conoce el número de estación a la cual debe ir. Cada vehículo se dirige a la estación correspondiente y espera a que lo atiendan. Una vez que le entregan el comprobante de verificación, el vehículo se retira. Considere que en cada estación se atienden a los vehículos de acuerdo con el orden de llegada. Nota: maximizar la concurrencia.
*/

// el resultado pone estacion como un proceso y un monitor admin
// CREO QUE ES PORQUE NINGUN AUTO PUEDE LLEGAR MIENTRAS SE ESTA ATENDIENDO

Monitor Estacion[id: 0..3]{
    cond vehiculos;
    bool libre = true;
    int esperando = 0;

    Procedure llegada(){
        if(not libre){
            wait(vehiculos);
            esperando ++;
        } else {
            libre = false;
        }
    }

    Procedure atencion(c: OUT String){
        generarComprobante(c);
        
        // siguiente
        if (esperando > 0){
            esperando --;
            signal(vehiculos);
        } else {
            libre = true;
        }
    }
}

Process Vehiculo[id: 0..74]{
    int nro;
    String comprobante;

    Estacion[nro].llegada();
    Estacion[nro].atencion(comprobante);
}