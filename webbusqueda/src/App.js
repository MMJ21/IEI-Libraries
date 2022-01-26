import logo from './logo.svg';
import './App.css';

const FetchBusqueda = () => {
  let localidadElement = document.getElementById("localidad_text")
  let cpElement = document.getElementById("cp_text")
  let provinciaElement = document.getElementById("provincia_text")
  let tipoElement = document.getElementById("tipo_select")
  let resultElement = document.getElementById("resultado")
  let request = {
    localidad: localidadElement !== null ? localidadElement.value : "",
    cP: cpElement !== null ? cpElement.value : "",
    provincia: provinciaElement !== null ? provinciaElement.value : "",
    tipo: tipoElement !== null ? tipoElement.value : ""
  }

  console.log(request)

  let text = ""

  fetch('https://localhost:7218/api/obtenerBibliotecas', {
      method: 'POST',      
      body: JSON.stringify(request),
      headers: {
        'Content-type': 'application/json',
      },
    }).then((res) => res.json())
      .then((result) => {
        result.forEach(element => {          
          text += 
            "Nombre: " + element.nombre + "<br>" +
            "Tipo: " + element.tipo + "<br>" + 
            "Teléfono: " +element.telefono + "<br>" +
            "Dirección: " +element.direccion + "<br>" +
            "Código Postal: " +element.codigoPostal + "<br>" +
            "Correo electrónico: " +element.email + "<br>" + "<br>"
        })
        
        if (text === "") {
          text = "No hay resultados"
        }

        resultElement !== null ? resultElement.innerHTML = text : console.log("ERROR")
      })
}

function App() {
  return (
    <div className="App">
      <h1>Buscador de bibliotecas</h1>
      <label for="localidad">Localidad:  </label>
      <input type="text" name="localidad" id="localidad_text"/>
      <br></br>
      <br></br>

      <label for="cp">Cód. Postal:  </label>
      <input type="text" name="cp" id="cp_text"/>
      <br></br>
      <br></br>

      <label for="provincia">Provincia:  </label>
      <input type="text" name="provincia" id="provincia_text"/>
      <br></br>
      <br></br>

      <label for="tipo">Tipo:  </label>
      <select name="tipo" id="tipo_select">
          <option value="publica">Pública</option>
          <option value="especializada">Especializada</option>
      </select>
      <br></br>
      <br></br>

      <input type="submit" value="Buscar" onClick={FetchBusqueda}/>

      <h2>Resultados de la búsqueda:</h2>

      <div id="resultado"></div>
    </div>
  );
}

export default App;
