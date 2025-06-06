{
En una clínica existe un médico de guardia que recibe continuamente peticiones de atención de las E enfermeras que trabajan en su piso y de las P personas que llegan a la clínica ser atendidos. 

Cuando una persona necesita que la atiendan espera a lo sumo 5 minutos a que el médico lo haga, si pasado ese tiempo no lo hace, espera 10 minutos y vuelve a requerir la atención del médico. Si no es atendida tres veces, se enoja y se retira de la clínica. Cuando una enfermera requiere la atención del médico, si este no lo atiende inmediatamente le hace una nota y se la deja en el consultorio para que esta resuelva su pedido en el momento que pueda (el pedido puede ser que el médico le firme algún papel). Cuando la petición ha sido recibida por el médico o la nota ha sido dejada en el escritorio, continúa trabajando y haciendo más peticiones.

El médico atiende los pedidos dándole prioridad a los enfermos que llegan para ser atendidos. Cuando atiende un pedido, recibe la solicitud y la procesa durante un cierto tiempo. Cuando está libre aprovecha a procesar las notas dejadas por las enfermeras.
}

// E enfermeras
// 1 Medico
// 1 Consultorio

Process Clinica is
    { especificación de tasks }
    Task Medico is
        entry AtencionP();
        entry AtencionE();
    End Medico;
    Task Consultorio is
        entry Nota(nota: in String);
        entry PedidoNota(nota: out String);
    End Consultorio;
    Task Type Enfermeras is
        entry Identificar(pos: in integer);
    End Enfermeras;
    Task Type Personas is
        entry Identificar(pos: in integer);
    End Personas;
    { declaración task types }
    arrEnfermeras: array(1..E) of Enfermeras;
    arrPersonas: array(1..P) of Personas;
    { cuerpo tasks }
    Task Body Personas is 
        pedidos: integer := 0;
        atendido: bool := false;
    Begin
        while (not atendido and pedidos < 3) loop
            select
                Medico.AtencionP();
                // atencion medico 
                atendido := true;
            delay or 300.0
                delay 6000.0;
                pedidos := pedidos + 1;
            end select;
        end loop; 
    End Personas;

    Task Body Enfermeras is
        nota: String;
    Begin
        loop
            select 
                Medico.AtencionE();
                // atencion medico 
            else
                nota := escribirNota(); // asumir que existe
                Consultorio.Nota(nota);
            end select;
        end loop;
    End Enfermeras;

    Task Body Consultorio is
        notas: queue;
    Begin
        loop
            select
                accept Nota(nota: in String) do
                    push(notas, nota);
                end Nota;
            or
                accept PedidoNota(nota: out String) do
                    if( empty(notas) ) then
                        nota := '';
                    else
                        pop(notas, nota);
                    end if;
                end Pedido;
            end select;
        end loop;
    End Consultorio;

    Task Body Medico is
        n: String;
    Begin
        loop
            select 
                accept AtencionP() do
                    delay 30.0;
                end AtencionP;
            or 
                when (AtencionP`count = 0) => accept AtencionE() do
                    delay 40.0;
                end AtencionE;
            else
                Consultorio.PedidoNota(n);
                if (n <> '') then
                    procesarNota(n); // asumir que existe
                end if;
            end select;
        end loop;
    End Medico;
Begin
    null;
End Clinica;