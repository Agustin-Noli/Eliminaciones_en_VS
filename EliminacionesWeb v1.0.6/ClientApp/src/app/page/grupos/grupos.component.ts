import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as xJS from '../../../locale.en.js'; 

@Component({
    selector: 'app-grupos',
    templateUrl: './grupos.component.html',
    styleUrls: ['./grupos.component.scss']
})
export class GruposComponent implements OnInit {

    rowSelection;
    defaultColDef
    showModal: boolean = false;
    showModeleliminar: boolean = false;
    showModelmodificar: boolean = false;
    showMsjemp: boolean = false;
    showMsj: boolean = false;
    showconf: boolean = false;
    showconfelim: boolean = false;
    showMsjemp1: boolean = false;
    showMsjmod: boolean = false;
    showconfr: boolean = false;
    success: boolean = false;
    successm: boolean = false;
    showgrupemp: boolean = false;
    showCmpemp: boolean = false;
    permitido: boolean = false;
    confiragregar: boolean = false;
    successelim: boolean = false;
    id; grupo; descripcion; obj;

    msjelim; msjelim1; title;
    grupom;
    descripcionm;
    gruprowData;
    prueba = [];

    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;


    columnDefs = [
        { headerName: 'Codigo', field: 'grupoId', sortable: true, filter: true, hide: true },
        { headerName: 'Grupo', field: 'grupoNombre', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Descripcion', field: 'grupoDescripcion', sortable: true, suppressMovable: true },
        { headerName: 'Codigo', field: 'secCodigo', sortable: true, suppressMovable: true, hide: true }


    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;
    rowData: any;
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
      
      let permit = (UserAD.usuPerfil == "Operador") ? false : true;

      this.permitido = permit
     

        if (route.snapshot.url[0] == undefined) {
            this.procesosServ.logo = true;
        } else {
            this.procesosServ.logo = false;
        }
      

        this.myformA = this.a.group({
            grupo: ["", Validators.required],
            descripcion: ["", Validators.required]

        });

        this.myformM = this.m.group({
            grupom: ["", Validators.required],
            descripcionm: ["", Validators.required]

        });

        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };
        this.rowSelection = 'single';

    }

    ngOnInit() {

        this.procesosServ.getGrupo()
            .subscribe((data) => {
                if (data != null) {
                    this.rowData = data
                }
            }, err => {
                if (err.status = 404) {
                    console.log(err);
                    this.rowData = ""
                }
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
            this.id = this.obj.grupoId
            this.grupo = this.obj.grupoNombre
            this.descripcion = this.obj.grupoDescripcion


        }

    }

    showModel() {
        this.confiragregar = true;
        this.success = false;
        this.myformA.reset()
        this.title = 'Agregar'
        this.showModal = true;
        this.showMsjemp = true;
    }

    enviar() {
        var data = {
            "grupoNombre": this.myformA.value.grupo,
            "grupoDescripcion": this.myformA.value.descripcion,
            "secCodigo": this.procesosServ.secCod
        }
        this.procesosServ.postGrupo(data).subscribe((res) => {
            if (res == res) {
                this.procesosServ.getGrupo()
                    .subscribe(res => {
                        this.rowData = res
                        this.success = true;
                        this.confiragregar = false;
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
        this.successelim = false
        this.showconfelim = true

        if (this.obj == undefined) {
            this.title = 'Atencion';
            this.showModeleliminar = true;
            this.showMsjemp = true;
            this.showMsjemp1 = false;
            this.showconf = false;
            this.showconfelim=false
            this.msjelim = "Debe seleccionar un grupo";

        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.showModeleliminar = true;
                this.showMsjemp = true;
                this.showMsjemp1 = false;
                this.showconf = false;
                this.showconfelim = false
                this.msjelim = "Debe seleccionar un grupo";

            } else {
                this.showModeleliminar = true;
                this.title = "Eliminar";
                this.showMsj = true;
                this.showconf = true;
                this.showMsjemp = false;
                this.showMsjemp1 = true;
                this.msjelim1 = "¿Desea eliminar el grupo "
            }
        }
    }

    enviaelim(id) {
        this.procesosServ.deleteGrupo(id).subscribe((id) => {
            if (id == id) {
                this.procesosServ.getGrupo()
                    .subscribe(res => {
                        this.rowData = res
                        this.id = ""
                        this.grupo = ""
                        this.descripcion = ""
                        this.successelim = true
                        this.showconfelim = false
                        setTimeout(() => {
                            this.showModeleliminar = false;
                        }, 2000);
                    }, err => {
                        console.log(err);

                    });
            }

        })
    }


    showModelm() {
        this.successm = false
        if (this.obj == undefined) {
            this.title = 'Atencion';
            this.showModelmodificar = true;
            this.showMsjemp = true;
            this.showMsjmod = false;
            this.showconfr = false;
            this.msjelim = "Debe seleccionar un grupo"
        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.showModelmodificar = true;
                this.showMsjemp = true;
                this.showMsjmod = false;
                this.showconfr = false;
                this.msjelim = "Debe seleccionar un grupo"

            } else {
                setTimeout(() => {
                    this.grupom = this.obj.grupoNombre
                    this.descripcionm = this.obj.grupoDescripcion
                }, 100);
                this.title = "Modificar";
                this.showModelmodificar = true;
                this.showMsjmod = true;
                this.showMsjemp = false;
                this.showconf = true;
            }
        }
    }

    enviamod() {


        let grup = ((this.myformM.value.grupom == null || this.myformM.value.grupom == "") ? this.obj.grupoNombre : this.myformM.value.grupom);
        let desc = ((this.myformM.value.descripcionm == null || this.myformM.value.descripcionm == "") ? this.obj.grupoDescripcion : this.myformM.value.descripcionm);


        var data = {
            "grupoId": this.obj.grupoId,
            "grupoNombre": grup,
            "grupoDescripcion": desc,
            "secCodigo": this.obj.secCodigo
        }

        this.procesosServ.putGrupo(data).subscribe((id) => {
            if (id == id) {
                this.procesosServ.getGrupo()
                    .subscribe(res => {
                        this.rowData = res
                        this.successm = true
                        setTimeout(() => {
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

    showGrupEmp() {

        if (this.obj == undefined) {
            this.title = 'Atencion';
            this.showgrupemp = true;
            this.showMsjemp = true;
            this.msjelim = "Debe seleccionar un grupo";
            this.showCmpemp = false;
            //   this.showMsjemp=true;


        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.showgrupemp = true;
                this.showMsjemp = true;
                this.msjelim = "Debe seleccionar un grupo";
                this.showCmpemp = false;

            } else {
                this.procesosServ.empgrupo = this.obj.grupoId
                this.procesosServ.grupoempresa = this.obj
                this.router.navigate(['GruposEmpresa']);


            }
        }

    }



}




