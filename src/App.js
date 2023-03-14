import './App.css';
import { useEffect } from "react";
import axios from 'axios';
function App() {
  useEffect(() => {
    //
    //    Upload File
    //
    var upload = document.getElementById('files');
    upload.addEventListener('change', () => {
      var tempMas = upload.value.split('\\');
      console.log(tempMas[tempMas.length - 1]);

      var s = this;
      const data = new FormData(document.getElementById('uploadForm'))
      var imagefile = document.querySelector('#files')
      console.log(imagefile.files[0])
      data.append('file', imagefile.files[0])
      axios.post('https://localhost:7174/api/controller/UploadFile', data, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
        .then(response => {
          console.log(response)
        })
        .catch(error => {
          console.log(error.response)
        })
    });
    //
    //    Get List Files
    //
    axios({
      method: 'get',
      url: `https://localhost:7174/api/controller/GetListFile`,
      dataType: "dataType",
      headers: {
        'Accept': '*/*',
        'Content-Type': 'application/json',
      }
    }).then(listFile => {
      console.log(listFile['data']);
      for (let i = 0; i < listFile['data'].length; i++) {
        var tr = document.createElement('tr');
        var tdName = document.createElement('td');
        tdName.textContent = listFile['data'][i]['nameFile'];
        tdName.id = i;
        tdName.className = 'NameTD';
        var tdLastUpdate = document.createElement('td');
        tdLastUpdate.textContent = listFile['data'][i]['lastModifay'];
        var tdSize = document.createElement('td');
        tdSize.textContent = listFile['data'][i]['sizeFile'];
        var tdDel = document.createElement('td');
        var buttonDelFile = document.createElement('button');
        buttonDelFile.textContent = 'Delete';
        buttonDelFile.id = i;
        //
        //    Delete File
        //
        buttonDelFile.addEventListener('click', () => {
          var listNameFiles = document.getElementsByClassName('NameTD');
          for (const iterator of listNameFiles) {
            if (iterator.id == i) {
              axios({
                method: 'post',
                url: `https://localhost:7174/api/controller/DeleteFile?fileName=${iterator.textContent}`,
                dataType: "dataType",
                headers: {
                  'Accept': '*/*',
                  'Content-Type': 'application/json',
                }
              }).then(listFile => {
                alert(`File: ${iterator.textContent} deleted!`);
                window.location.reload();
              });
            }
          }
        });
        tdDel.appendChild(buttonDelFile);
        var tdSave = document.createElement('td');
        var buttonSaveFile = document.createElement('button');
        buttonSaveFile.id = i;
        buttonSaveFile.textContent = 'Save';
        //
        //    Save File
        //
        buttonSaveFile.addEventListener('click',()=>{
          var listNameFiles = document.getElementsByClassName('NameTD');
          for (const iterator of listNameFiles) {
            if (iterator.id == i) {
              axios({
                method: 'post',
                url: `https://localhost:7174/api/controller/DownloadFile?name=${iterator.textContent}`,
                dataType: "dataType",
                headers: {
                  'Accept': '*/*',
                  'Content-Type': 'application/json',
                }
              }).then(listFile => {
                alert(`File: ${iterator.textContent} save!`);
              });
            }
          }
        });
        tdSave.appendChild(buttonSaveFile);
        tr.appendChild(tdName);
        tr.appendChild(tdLastUpdate);
        tr.appendChild(tdSize);
        tr.appendChild(tdDel);
        tr.appendChild(tdSave);
        document.getElementById('tablebody').append(tr);
      }
      for (const iterator of listFile['data']) {

      }
    });
  });
  return (
    <div>
      <form id='uploadForm' name="uploadForm" encType="multipart/form-data">
        <input id="files" name='files' type="file" multiple />
      </form>
      <div className='table'>
        <table className="table table-striped">
          <thead>
            <tr>
              <th scope="col">Name File</th>
              <th scope="col">Change</th>
              <th scope="col">Size</th>
              <th scope="col">Delete</th>
              <th scope="col">Save File</th>
            </tr>
          </thead>
          <tbody id="tablebody">
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default App;
