{
En un sistema para acreditar carreras universitarias, hay UN Servidor que atiende pedidos de U Usuarios de a uno a la vez y de acuerdo con el orden en que se hacen los pedidos. Cada usuario trabaja en el documento a presentar, y luego lo envía al servidor; espera la respuesta de este que le indica si está todo bien o hay algún error. Mientras haya algún error, vuelve a trabajar con el documento y a enviarlo al servidor. Cuando el servidor le responde que está todo bien, el usuario se retira. Cuando un usuario envía un pedido espera a lo sumo 2 minutos a que sea recibido por el servidor, pasado ese tiempo espera un minuto y vuelve a intentarlo (usando el mismo documento).
}

Procedure Sistema is
{ especificacion tasks }
    Task Servidor is
        entry PresentarDocumento(doc: in String; errores: out String);
    end Servidor;
    Task Type Usuarios;
{ declaracion variables tasktype }
    arrUsuarios: array (1..U) of Usuarios;
{ cuerpo tasks }
    Task Body Usuarios is
        doc, errores: String := "";
        ok, corregir: bool := false;
    begin
        doc := trabajarDocumento(""); //asumir que existe
        while (not ok) loop
            if (corregir) then
                doc := trabajarDocumento(errores); //asumir que existe
                corregir := false;
            end if;
            select
                Sistema.PresentarDocumento(doc, errores);
                if(errores = "") then
                    ok := true;
                else 
                    corregir := true;
                end if;
            delay 120.0
                delay 60.0;
            end select;
        end loop;
    end Usuarios;

    Task Body Servidor is

    begin
        loop
            accept trabajarDocumento(doc, errores) doc
                errores := corregirDocumento(doc); // si esta correcto devuelve errores vacios, sino string de errores
            end TrabajarDocumento;
        end loop;
    end Servidor`
begin
    
end Sistema;