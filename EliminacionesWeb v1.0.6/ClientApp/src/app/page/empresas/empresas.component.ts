import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { typeWithParameters } from '@angular/compiler/src/render3/util';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute,Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';

@Component({
    selector: 'app-empresas',
    templateUrl: './empresas.component.html',
    styleUrls: ['./empresas.component.scss']
})
export class EmpresasComponent implements OnInit {


    //Variables
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
    successm: boolean = false;
    successe: boolean = false;
    permitido: boolean = false;
    confirmar: boolean = false;
    id = ''; descripcion = ""; msjelim = ""; msjelim1 = ""; title = "";
    obj;
    desc = '';
    empresa = '';
    //Variables Form modificacion
    empresam = '';
    intercompanym = '';
    porcentajem = '';
    seC_CODIGO = '';

    myModal;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;


    columnDefs = [
        { headerName: 'Codigo', field: 'empCodigo', sortable: true, filter: true, hide: true },
        { headerName: 'Empresa', field: 'empDescripcion', width: 391, sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Intercompany', field: 'empIntercompany', width: 391, sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Porcentaje', field: 'empPorcentaje', width: 391, sortable: true, resizable: false, suppressMovable: true },
        { headerName: 'Sec codigo', field: 'secCodigo', width: 391, sortable: true, resizable: true, suppressMovable: true, hide: true }

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
            empresa: ["", Validators.required],
            intercompany: ["", Validators.required],
            porcentaje: ["", Validators.required]
        });

        this.myformM = this.m.group({
            empresam: ["", Validators.required],
            intercompanym: ["", Validators.required],
            porcentajem: ["", Validators.required]
        });

        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };
        this.rowSelection = 'single';
    }

    ngOnInit() {

        this.procesosServ.getEmp()
            .subscribe((data) => {
                if (data.length == 0) {
                } else {
                    this.rowData = data
                }
            }, err => {
                console.log(err)
            }

            );

    }
    onSelectionChanged(e) {

        const selectedNodes = this.agGrid.api.getSelectedNodes();
        // const selectedData = selectedNodes.map( node => node.data );
        // const selectedDataStringPresentation = selectedData.map( node => node.make + ' ' + node.model).join(', ');
        this.obj = selectedNodes
        if (this.obj.length == 0) {
            this.obj = undefined;

        } else {
            this.obj = selectedNodes[0].data
            this.id = this.obj.empCodigo;
            this.desc = this.obj.empDescripcion;
            this.myformM.value.empresam = this.obj.empDescripcion
            this.myformM.value.intercompanym = this.obj.empIntercompany
            this.myformM.value.porcentajem = this.obj.empPorcentaje
        }

    }


  showModel() {
    this.confirmar = true;
        this.success = false;
        this.myformA.reset()
        this.title = 'Agregar'
        this.showModal = true;
        this.showMsjemp = true;
    }

    enviar() {
        var data = {
            "empDescripcion": this.myformA.value.empresa,
            "empIntercompany": this.myformA.value.intercompany,
            "empPorcentaje": this.myformA.value.porcentaje,
          "secCodigo": this.procesosServ.secCod
        }

        this.procesosServ.postEmp(data).subscribe((res) => {
            if (res == res) {
                this.procesosServ.getEmp()
                    .subscribe(res => {
                        this.rowData = res;
                      this.success = true;
                      this.confirmar = false;
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
      this.showconf = true
        if (this.obj == undefined) {

            this.title = 'Atencion';
            this.showModeleliminar = true;
            this.showMsjemp = true;
            this.showMsjemp1 = false;
            this.showconf = false;
            this.msjelim = "Debe seleccionar una Empresa"
        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.showModeleliminar = true;
                this.showMsjemp = true;
                this.showMsjmod = false;
                this.showconf = false;
                this.msjelim = "Debe seleccionar una Empresa"

            } else {
                this.showModeleliminar = true;
                this.title = "Eliminar";
                this.showMsj = true;
                this.showconf = true;
                this.showMsjemp = false;
                this.showMsjemp1 = true;
                this.msjelim1 = "¿Desea eliminar la Empresa ";
            }
        }
    }

    enviaelim(id) {
        this.procesosServ.deleteEmp(id).subscribe((id) => {
            if (id == id) {
                this.procesosServ.getEmp()
                    .subscribe(res => {
                        this.rowData = res
                      this.successe = true
                      this.showconf = false
                        setTimeout(() => {
                            this.myformA.reset();
                            this.obj = undefined
                            this.showModeleliminar = false;
                        }, 1500);

                    }, err => {
                        console.log(err);

                    });
            }

        })
    }

    showModelm() {
      this.successm = false;
      this.showconfr = true;
        if (this.obj == undefined) {

            this.title = 'Atencion';
            this.showModelmodificar = true;
            this.showMsjemp = true;
            this.showMsjmod = false;
            this.showconfr = false;
            this.msjelim = "Debe seleccionar una Empresa"
        } else {
            setTimeout(() => {
                this.empresam = this.obj.empDescripcion
                this.intercompanym = this.obj.empIntercompany
                this.porcentajem = this.obj.empPorcentaje
            }, 100);
            this.title = "Modificar";
            this.showModelmodificar = true;
            this.showMsjmod = true;
            this.showMsjemp = false;
            this.showconfr = true;
        }

    }
    enviamod() {
        let desc = ((this.myformM.value.empresam == null || this.myformM.value.empresam == "") ? this.obj.empDescripcion : this.myformM.value.empresam);
        let intrc = ((this.myformM.value.intercompanym == null || this.myformM.value.intercompanym == "") ? this.obj.empIntercompany : this.myformM.value.intercompanym);
        let porc = ((this.myformM.value.porcentajem == null || this.myformM.value.porcentajem == "") ? this.obj.empPorcentaje : this.myformM.value.porcentajem);


        var data = {
            "empCodigo": this.obj.empCodigo,
            "empDescripcion": desc,
            "empIntercompany": intrc,
            "empPorcentaje": porc,
          "secCodigo": this.procesosServ.secCod
        }

        this.procesosServ.putEmp(data).subscribe((res) => {
            if (res == res) {
                this.procesosServ.getEmp()
                    .subscribe(res => {
                        this.rowData = res
                      this.successm = true;
                      this.showconfr = false;
                        setTimeout(() => {
                            this.empresam = "";
                            this.intercompanym = "";
                            this.porcentajem = "";
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
