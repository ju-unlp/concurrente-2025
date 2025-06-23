/*
Resuelva con ADA el siguiente problema: la página web del Banco Central exhibe las diferentes cotizaciones del dólar oficial de 20 bancos del país, tanto para la compra como para la venta. Existe una tarea programada que se ocupa de actualizar la página en forma periódica y para ello consulta la cotización de cada uno de los 20 bancos. Cada banco dispone de una API, cuya única función es procesar las solicitudes de aplicaciones externas. La tarea programada consulta de a una API por vez, esperando a lo sumo 5 segundos por su respuesta. Si pasado ese tiempo no respondió, entonces se mostrará vacía la información de ese banco.

*/

Process BancoCentral
    //
    Task Type Banco id
        entry pedirCotizacion(compra, venta: OUT String);
    end Banco;
    Task Tarea;
    //
    arrBancos: array(1..20) of Banco;
    //
    Task Body Banco is
        compra, venta: int;
    begin
        loop 
            accept pedirCotizacion(compra, venta: OUT String) do
                compra := generarCompra();
                venta := generarVenta();
            end pedirCotizacion;
        end loop
    end Banco;

    Task Body Tarea is
        cotizaciones: array(1.20, 1..2) of String;
        dC, dV: int;
    begin
        for (1..20) loop
            select
                Banco[i].pedirCotizacion(dC, dV);
                cotizaciones[i, 1] := dC;
                cotizaciones[i, 2] := dV; 
            or delay 5.0
                cotizaciones[i, 1] := "";
                cotizaciones[i, 2] := "";   
        end loop;
    end Tarea;
begin

end;