import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResolvedStaticSymbol } from '@angular/compiler';
import { resolveForwardRef } from '@angular/compiler/src/util';
import * as xJS from '../../../locale.en.js';

@Component({
    selector: 'app-relacionrubros',
    templateUrl: './relacionrubros.component.html',
    styleUrls: ['./relacionrubros.component.scss']
})
export class RelacionrubrosComponent implements OnInit {
    showModal: boolean = false;
    showModalm: boolean = false;
    showModale: boolean = false;
    eliminar: boolean = false;
    default: boolean = false;
    confirmar: boolean = false;
    successelim: boolean = false;
    permitido: boolean = false;
    confirmarguardar: boolean = false;
    error: boolean = false;
    showMsjmod: boolean = false;
    showconf: boolean = false;
    defaultColDef;
    rowSelection;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;
    title;
    mesaje;
    success;
    id;
    //selectedRubCabecera: any;
    //selectedGranRub: any;
    //selectedSubRub: any;
    //selectedRubro: any;
    rubroselect;
    subrubro;
    rubcab;
    granrub;
    // 
    columnDefs = [
        { headerName: 'Cod. Cabecera', field: 'rubcabCodigo', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Rubro Cabecera', field: 'rubcabDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Cod. Gran rubro', field: 'granrubCodigo', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Gran Rubro', field: 'granrubDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Cod. Subrubro ', field: 'subrubId', sortable: true, filter: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Sub Rubro ', field: 'subrubDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Cod. Rubro BCRA ', field: 'rubCodigo', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Rubro BCRA', field: 'rubroDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true }
    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;
    obj;
    rowData: any;
    myform: FormGroup
    constructor(private http: HttpClient, private procesosServ: ProcesosService, private a: FormBuilder, private router: Router, private route: ActivatedRoute) {

        //rowData = [
        //  { make: 'Toyota', model: 'Celica', price: 35000 },
        //  { make: 'Ford', model: 'Mondeo', price: 32000 },
        //  { make: 'Porsche', model: 'Boxter', price: 72000 }
        //];



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


        this.formulario();

        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };
        this.rowSelection = 'single';

    }

    ngOnInit() {
        this.procesosServ.getrelacionRubros()
            .subscribe((res) => {
                if (res.length != 0) {
                    this.rowData = res

                } else {
                    this.rowData = ""
                }
            }, err => {
                console.log(err)
            }

            );


        // Rubro Cabecera
        this.procesosServ.getrubroscabecera()
            .subscribe((res) => {
                if (res.length == 0) {
                    this.rubcab = ""
                } else {
                    this.rubcab = res
                }
            }, err => {
                console.log(err)
            }

            );

        //Gran Rubro
        this.procesosServ.getgrandesrubros()
            .subscribe((res) => {
                if (res.length == 0) {
                    this.granrub = ""
                } else {
                    this.granrub = res
                }
            }, err => {
                console.log(err)
            }

            );

        //Subrubro

        this.procesosServ.getsubrubros()
            .subscribe((res) => {
                if (res.length == 0) {
                    this.subrubro = ""
                } else {
                    this.subrubro = res
                }
            }, err => {
                console.log(err)
            }

            );

        //Rubro
        this.procesosServ.getRubros()
            .subscribe((res) => {
                if (res.length == 0) {
                    this.rubroselect = ""
                } else {
                    this.rubroselect = res
                }
            }, err => {
                console.log(err)
            }

            );
    }


    formulario() {
        this.myform = this.a.group({
            rubCabecera: [null, Validators.required],
            granRubro: [null, Validators.required],
            subRubro: [null, Validators.required],
            Rubro: [null, Validators.required]

        });
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
        }

    }

    showmodal() {
        this.success = false
        this.confirmarguardar = false
        this.error = false
        this.formulario()
        this.showModal = true
        this.title = "Asociar Subrubro ";

    }

    guardar() {
        debugger
        let data = {
            "rubcabCodigo": this.myform.value.rubCabecera,
            "granrubCodigo": this.myform.value.granRubro,
            "subrubId": this.myform.value.subRubro,
            "rubCodigo": this.myform.value.Rubro,
            "activo": ""
        }

        this.procesosServ.postrelacionRubros(data).subscribe(res => {

            if (res == res) {
                this.procesosServ.getrelacionRubros()
                    .subscribe(res => {
                        this.rowData = res
                        this.success = true
                        this.error = false;
                        this.confirmarguardar = true
                        setTimeout(() => {
                            this.success = false
                            this.showModal = false
                        }, 1500);
                    }, err => {

                        if (err.status === 409) {
                            this.error = true;
                        }
                        console.log(err);

                    });
            }

        }, err => {

            if (err.status === 409) {
                this.error = true
                this.confirmarguardar = false
            }
            console.log(err);
        })



    }

    showmodale() {
        this.confirmar = false
        this.error = false
        if (this.obj == undefined) {

            this.title = 'Atencion';
            this.eliminar = true
            this.default = false
            this.mesaje = "Debe selecionar una relacion de rubros"
            this.confirmar = true
            this.showModale = true
        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.eliminar = true
                this.default = false
                this.mesaje = "Debe selecionar una relacion de rubros"
                this.showModale = true
                this.confirmar = true

            } else {
                this.title = 'Eliminar';
                this.default = true
                this.mesaje = "Desea eliminar la relacion seleccionada"
                this.showModale = true
            }
        }


    }
    confirmareliminacion() {
        let data = {
            "rubcabCodigo": this.obj.rubcabCodigo,
            "granrubCodigo": this.obj.granrubCodigo,
            "subrubId": this.obj.subrubId,
            "rubCodigo": this.obj.rubCodigo,
            "activo": "S"
        }

        this.procesosServ.deleterelacionRubroso(data).subscribe(res => {
            if (res == res) {
                this.procesosServ.getrelacionRubros()
                    .subscribe(res => {
                        this.rowData = res
                        this.successelim = true
                        this.confirmar = true
                        setTimeout(() => {
                            this.successelim = false
                            this.showModale = false
                        }, 1500);
                    }, err => {
                        console.log(err);

                    });
            }

        }), err => {
            console.log(err);
        }
    }


    agregarRubro() {

        this.router.navigate(['Subrubros']);

    }


    //showModelm() {
    //    this.showconf = false

    //    if (this.obj == undefined) {
    //        debugger
    //        this.title = 'Atencion';
    //        this.eliminar = true
    //        this.showMsjemp = true;
    //        this.showMsjmod = false;
    //        this.mesaje = "Debe selecionar un Registro"

    //        this.showModalm = true
    //    } else {
    //        if (this.id == "") {
    //            this.title = 'Atencion';
    //            this.eliminar = true
    //            this.showMsjemp = true;
    //            this.mesaje = "Debe selecionar un Registro"
    //            this.showModalm = true


    //        } else {
    //            //setTimeout(() => {
    //            // this.subrubrom = this.obj.grupoNombre

    //            //}, 100);
    //            this.title = "Modificar";
    //            this.showModalm = true;
    //            this.showMsjmod = true;
    //            this.showMsjemp = false;
    //            this.showconf = true;
    //        }
    //    }

    //}



}
