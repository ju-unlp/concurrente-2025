/*
Resolver con ADA el siguiente problema. La oficina central de una empresa de venta de indumentaria debe calcular cuántas veces fue vendido cada uno de los artículos de su catálogo. La empresa se compone de 100 sucursales y cada una de ellas maneja su propia base de datos de ventas. La oficina central cuenta con una herramienta que funciona de la siguiente manera: ante la consulta realizada para un artículo determinado, la herramienta envía el identificador del artículo a cada una de las sucursales, para que cada uno de éstas calcule cuántas veces fue vendido en ella. Al final del procesamiento, la herramienta debe conocer cuántas veces fue vendido en total, considerando todas las sucursales. Cuando ha terminado de procesar un artículo comienza con el siguiente (suponga que la herramienta tiene una función generarArtículo que retorna el siguiente ID a consultar). Nota: maximizar la concurrencia. Supongo que existe una función ObtenerVentas(ID) que retorna la cantidad de veces que fue vendido el artículo con identificador ID en la base de datos de la sucursal que la llama.
*/


Process
    Task type Sucursales is
        entry ventaArticulo(id: in int, ventas: out int);
    end sucursales;
    Task Herramienta;
    //
    arrSucursales: array(1..100) of Sucursal;
    //
    Task Body Herramienta is
        int idArt, total, venta;
    begin
        idArt := generarArtículo();
        while (generarArtículo <> null) loop
            for int i (1..100) loop
                Sucursal[i].ventaArticulo(idArt, venta);
                total := total + venta;
            end loop;
            mostrar(id, total);
            idArt := generarArtículo();
        end loop;
    end Herramienta;
    Task Body Sucursal is
        id, ventas: int;
    begin
        while true{
            accept ventaArticulo(id, ventas: out int) do
                ventas := ObtenerVentas(id);
            end ventaArticulo;
        }
    end Sucursal;
begin

end;