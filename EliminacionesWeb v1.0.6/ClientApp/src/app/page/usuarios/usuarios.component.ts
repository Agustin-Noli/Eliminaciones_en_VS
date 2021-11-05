import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { IUsuarios } from 'src/app/Model/Usuarios';
import { Time } from '@angular/common';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute,Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';

@Component({
    selector: 'app-usuarios',
    templateUrl: './usuarios.component.html',
    styleUrls: ['./usuarios.component.scss'],
    providers: [DatePipe]
})
export class UsuariosComponent implements OnInit {
    rowSelection;
    defaultColDef
    showModal: boolean = false;
    showModeleliminar: boolean = false;
    showModelmodificar: boolean = false;
    showMsjemp: boolean = false;
    showMsj: boolean = false;
    showconf: boolean = false;
    showMsjemp1: boolean = false;
    showMsjmod: boolean = false;
    showconfr: boolean = false;
    success: boolean = false;
    successe: boolean = false;
    permitido: boolean = false;
    myDate = new Date();
    data;
    fecha; obj;
    legajo; user; perfil; msjelim; msjelim1; title;
    userm; perfilm;
    camposelect = "Selecciona una opción";
    prueba = [];
    dataperfil;
    perfilmod;

    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;


    columnDefs = [

        { headerName: 'Legajo', field: 'usuLegajo', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Usuario', field: 'usuNombre', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'perCodigo', field: 'perCodigo', sortable: true, hide: true },
        { headerName: 'Perfil', field: 'usuPerfil', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Fecha Alta', field: 'usuFecalta', sortable: true, hide: true },
        { headerName: 'Codigo', field: 'secCodigo', sortable: true, hide: true }



    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;
    public rowData = [];


    // private http: HttpClient,
    myformA: FormGroup
    myformM: FormGroup
  constructor(private procesosServ: ProcesosService, private a: FormBuilder, private m: FormBuilder, private route: ActivatedRoute, private router: Router) {
      var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));

      if (UserAD == undefined || UserAD == null) {

        this.procesosServ.getacceso()
        UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
        if (UserAD == null) {
          this.router.navigate(['']);
        }
      }
    
    //permisos
    //grupoAD: "BPMDesa"
    //secCodigo: 1
    //secDescripcion: "Grupo Financiero"
    //usuLegajo: "l0694401"
    //usuNombre: "Agustin Ignacio Noli"
    //usuPerfil: "Operador"
    let permit = (UserAD.usuPerfil == "Operador") ? false : true;
     
    this.permitido = permit

      if (route.snapshot.url[0] == undefined) {
            this.procesosServ.logo = true;
        } else {
            this.procesosServ.logo = false;
        }
   
        this.initmyformA();

    
        this.myformM = this.m.group({
            userm: ["", Validators.required],
            perfilm: ["", Validators.required]
        });


        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };
        this.rowSelection = 'single';
        // this.rowData= this.procesosServ.pruebausers();

    }
    ngOnInit() {

        this.procesosServ.getUsers()
            .subscribe((data) => {
                if (data != null) {

                    this.rowData = data
                }
            }, err => {

                console.log(err)

            }
            );


      this.procesosServ.getPerfil()
        .subscribe((data) => {
          if (data != null) {

            this.dataperfil = data
            console.log(this.dataperfil )
          }
        }, err => {

          console.log(err)

        }
        );



    }

    onSelectionChanged(e) {


        let selectedNodes = this.agGrid.api.getSelectedNodes();
        // const selectedData = selectedNodes.map( node => node.data );
        // const selectedDataStringPresentation = selectedData.map( node => node.make + ' ' + node.model).join(', ');

        this.obj = selectedNodes
        if (this.obj.length == 0) {
            this.obj = undefined;
        } else {
            this.obj = selectedNodes[0].data
            this.user = this.obj.usuNombre
            this.myformM.value.userm = this.obj.usuNombre
          this.myformM.value.perfilm = this.obj.usuPerfil
          this.perfilm = this.obj.usuPerfil

        }

    }

  initmyformA() {
  
    this.myformA = this.a.group({
      legajo: ["", Validators.required],
      user: ["", Validators.required],
      perfil: ["", Validators.required]
    });
  }
     

    showModel() {
        //this.myformA.reset();
        this.initmyformA();
        this.success = false;
        this.title = 'Agregar'
        this.showModal = true;
        this.showMsjemp = true;
    }

  enviar() {

      let perfil = ((this.myformA.value.perfil == 'Operador') ? 1 : 2);
      var data = {
        "usuLegajo": this.myformA.value.legajo,
        "usuNombre": this.myformA.value.user,
        "usuPerfil": perfil,
        "usuFecalta": this.myDate,
        "secCodigo": this.procesosServ.secCod
      }

    
      this.procesosServ.postUsuers(data).subscribe((res) => {
        if (res == res) {
          this.procesosServ.getUsers()
            .subscribe(res => {
              this.rowData = res
              this.success = true;
              setTimeout(() => {
                this.showModal = false;
              }, 2000);
            }, err => {
              console.log(err);
            });
        }

      })
    }


    showModele() {
        this.successe = false
        if (this.obj == undefined) {
            this.title = 'Atencion';
            this.showModeleliminar = true;
            this.showMsjemp = true;
            this.showMsjemp1 = false;
            this.showconf = false;
            this.msjelim = "Debe seleccionar un usuario";

        } else {
            this.title = "Eliminar";
            this.showModeleliminar = true;
            this.showMsj = true;
            this.showconf = true;
            this.showMsjemp = false;
            this.showMsjemp1 = true;
            this.msjelim1 = "¿Desea eliminar el usuario "
        }
        // }
    }

    enviaelim() {

        var id = this.obj.usuLegajo
        this.procesosServ.deleteUsers(id).subscribe((id) => {

            if (id == id) {
                this.procesosServ.getUsers()
                    .subscribe(res => {
                        this.rowData = res
                      this.successe = true
                      this.showconf = false
                        setTimeout(() => {
                            this.legajo = ""
                            this.user = ""
                            this.perfil = ""
                            this.fecha = ""
                            this.myformA.reset();
                            this.obj = undefined
                            this.showModeleliminar = false;
                        }, 2500);
                    }, err => {
                        console.log(err);
                    });
            }
        })
    }


  showModelm() {
    this.success = false
    if (this.obj == undefined) {
      this.title = 'Atencion';
      this.showModelmodificar = true;
      this.showMsjemp = true;
      this.showMsjmod = false;
      this.showconf = false;
      this.msjelim = "Debe seleccionar un usuario"
    }
    else {
      setTimeout(() => {
        this.userm = this.obj.usuNombre
        this.perfilm = this.obj.usuPerfil
      }, 100);

      this.title = "Modificar";
      this.showModelmodificar = true;
      this.showMsjmod = true;
      this.showMsjemp = false;
      this.showconf = true;
    }
  }

  enviamod() {
        let user = ((this.myformM.value.userm == null || this.myformM.value.userm == "") ? this.obj.usuNombre : this.myformM.value.userm);
        let perfl = ((this.myformM.value.perfilm == "Operador" ) ? 1 : 2);

        var data = {
            "usuLegajo": this.obj.usuLegajo,
            "usuNombre": user,
            "perCodigo": perfl,
            "usuFecalta": this.obj.usuFecalta,
           "secCodigo": this.procesosServ.secCod
        }


     // console.log(data)
        this.procesosServ.putUsers(data).subscribe((id) => {
            if (id == id) {
                this.procesosServ.getUsers()
                    .subscribe(res => {
                        this.rowData = res
                      this.success = true
                      this.showconf = false
          
                        setTimeout(() => {
                            this.userm = "";
                            this.perfilm = "";
                            this.obj = undefined
                            this.myformM.reset()
                            this.showModelmodificar = false;
                        }, 1500);


                    }, err => {
                        console.log(err);

                    });
            }

        })
    }


}
