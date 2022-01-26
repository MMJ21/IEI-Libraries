import logo from './logo.svg';
import './App.css';
import { useState, useEffect } from 'react'

function App() {
  const [ catState, setCatState] = useState(false)
  const [ eusState, setEusState] = useState(false)
  const [ valState, setValState] = useState(false)

  const SelectComunities = () => {
    if (document.getElementById("all_checkbox").checked) {
      document.getElementById("cat_checkbox").checked = true;
      document.getElementById("cat_checkbox").disabled = true;
      setCatState(true)
      document.getElementById("eus_checkbox").checked = true;
      document.getElementById("eus_checkbox").disabled = true;
      setEusState(true)
      document.getElementById("val_checkbox").checked = true;
      document.getElementById("val_checkbox").disabled = true;
      setValState(true)
    }
    else {
      document.getElementById("cat_checkbox").disabled = false;
      document.getElementById("eus_checkbox").disabled = false;
      document.getElementById("val_checkbox").disabled = false;
    }
  }

  const ChangeCat = () => {
    let element = document.getElementById("cat_checkbox")
    if (element !== null){
      setCatState(element.checked)
    }
  }

  const ChangeEus = () => {
    let element = document.getElementById("eus_checkbox")
    if (element !== null){
      setEusState(element.checked)
    }
  }

  const ChangeVal = () => {
    let element = document.getElementById("val_checkbox")
    if (element !== null){
      setValState(element.checked)
    }
  }

  const FetchCarga = () => {
    if (!catState && !eusState && !valState) {
      alert("Debes seleccionar al menos una comunidad.")
    }
    else {
      const element = document.getElementById("result_area")
      const bodyObject = { 
        Cat: catState,
        Eus: eusState, 
        Val: valState
      }
      if (valState) {
        alert("Ejecutando Selenium para obtener coordenadas en otra ventana. Espere, por favor.")
      } 
      fetch('https://localhost:7272/api/carga/cargarBibliotecas', {
        method: 'POST',      
        body: JSON.stringify(bodyObject),
        headers: {
          'Content-type': 'application/json',
        },
        mode: 'cors'
      }).then((res) => res.json())
        .then((result) => element !== null ? element.innerHTML = result : console.log(result))
    }
  }
  
  return (
    <div className="App">
      <h1> <b>CARGA DE BIBLIOTECAS</b> </h1>
      <input type="checkbox" id="all_checkbox" onChange={SelectComunities}/> <label for="all_checkbox"> TODAS LAS COMUNIDADES</label><br></br>
      <input type="checkbox" id="cat_checkbox" onChange={ChangeCat}/> <label for="cat_checkbox"> Catalunya</label><br></br>
      <input type="checkbox" id="eus_checkbox" onChange={ChangeEus}/> <label for="eus_checkbox"> Euskadi</label><br></br>
      <input type="checkbox" id="val_checkbox" onChange={ChangeVal}/> <label for="val_checkbox"> Comunitat Valenciana</label><br></br><br></br>
      <input type="button" id="boton_cargar" onClick={FetchCarga} value="Cargar"/><br></br><br></br><br></br>
      <div>
        <h3> Resultados de la carga: </h3>
        <div id="result_area" style={{display: 'inline-block', padding: '5px', border: 'solid'}}> Seleccione las comunidades a cargar y aquí visualizará el resultado de la carga. </div>
      </div>
    </div>
  );
}

export default App;