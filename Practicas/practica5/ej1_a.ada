/*
Se requiere modelar un puente de un único sentido que soporta hasta 5 unidades de peso. El peso de los vehículos depende del tipo: cada auto pesa 1 unidad, cada camioneta pesa 2 unidades y cada camión 3 unidades. Suponga que hay una cantidad innumerable de vehículos (A autos, B camionetas y C camiones). Analice el problema y defina qué tareas, recursos y sincronizaciones serán necesarios/convenientes para resolver el problema.
    a. Realice la solución suponiendo que no se tiene ningún orden ni prioridad entre los diferentes tipos de vehículos.

    b. Modifique la solución de (a) para que tengan mayor prioridad los camiones que el resto de los vehículos.
*/

Procedure EjPuenteA is
    /* especificación tasks */
    Task type Autos;
    Task type Camionetas;
    Task type Camiones;
    Task Puente is
        Entry entrarAuto();
        Entry entrarCamioneta();
        Entry entrarCamion();
        Entry salirAuto();
        Entry salirCamioneta();
        Entry salirCamion();
    End Puente;
    /* declaración variables task type */
    arrAutos: array(1..A) of Autos;
    arrCamionetas: array(1..B) of Camionetas;
    arrCamiones: array(1..C) of Camiones;
    /* cuerpo tasks */
    Task Body Autos is
    Begin
        Puente.entrarAuto();
        Puente.salirAuto();
    End Autos;
    Task Body Camionetas is 
    Begin 
        Puente.entrarCamioneta();
        Puente.salirCamioneta();
    End Camionetas;
    Task Body Camiones is
    Begin 
        Puente.entrarCamion();
        Puente.salirCamion();
    End Camiones; 
    Task Body Puente is 
        pesoTotal = 0;
    Begin 
        loop
            select
                Accept salirAuto() is
                    pesoTotal := pesoTotal-1;
                End salirAuto;
            or
                Accept salirCamioneta() is
                    pesoTotal := pesoTotal-2;
                End salirCamioneta;
            or 
                Accept salirCamion() is
                    pesoTotal := pesoTotal-3;
                End salirCamion;
            or 
                when (pesoTotal <= 4) => Accept entrarAuto() is 
                                            pesoTotal =: pesoTotal + 1;
                                        End entrarAuto;
            or 
                when (pesoTotal <= 3) => Accept entrarCamioneta() is 
                                            pesoTotal =: pesoTotal + 2;
                                        End entrarCamioneta;
            or 
                when (pesoTotal <= 2) => Accept entrarCamion() is 
                                            pesoTotal =: pesoTotal + 3;
                                        End entrarCamion;
            end select;
        end loop;
    End Puente;
Begin
    null;
End EjPuenteA;