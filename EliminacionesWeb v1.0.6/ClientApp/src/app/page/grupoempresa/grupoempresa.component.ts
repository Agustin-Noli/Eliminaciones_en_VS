import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute ,Router} from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResolvedStaticSymbol } from '@angular/compiler';
import { resolveForwardRef } from '@angular/compiler/src/util';
import * as xJS from '../../../locale.en.js';

@Component({
  selector: 'app-grupoempresa',
  templateUrl: './grupoempresa.component.html',
  styleUrls: ['./grupoempresa.component.scss']
})
export class GrupoempresaComponent implements OnInit {
  Empresa ;
  showModal:boolean=false;
  showModale:boolean=false;
  eliminar:boolean=false;
  default:boolean=false;
  confirmar:boolean=false;
  successelim: boolean = false;
  permitido: boolean = false;
  confirmarguardar: boolean = false;
  error: boolean = false;
  defaultColDef;
  rowSelection;
@ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;
  title = this.procesosServ.grupoempresa.grupoNombre;
  mesaje;
  success;
  id;
  msjemp;
  // 
  columnDefs = [
      {headerName: 'Codigo', field: 'empCodigo', sortable: true, filter: true,hide:true},
      {headerName: 'Empresas asociadas al grupo: '+this.title, field: 'empDescripcion', sortable: true, filter: true, resizable: true,suppressMovable:true },
      {headerName: 'SecCodigo', field: 'secCodigo', sortable: true, resizable: true,suppressMovable:true,hide:true}
  ];
  public localeText = xJS.AG_GRID_LOCALE_EN;
  obj;emp;
  rowData: any;
  myform:FormGroup
  constructor(private http: HttpClient, private procesosServ: ProcesosService, private a: FormBuilder, private router: Router,private route:ActivatedRoute) {

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

    if(route.snapshot.url[0]== undefined){
      this.procesosServ.logo=true;
    }else {
      this.procesosServ.logo=false;
    }
 

    this.formulario();

    this.defaultColDef = {
      flex: 1,
      minWidth: 100,
    };
    this.rowSelection = 'single';
     
   }

  ngOnInit() {
           this.procesosServ.GetEmpresasByGrupo()
           .subscribe((res) => {
             if(res.length != 0){
               this.rowData =res
             }else{
               this.rowData =""
                 }
               }  , err => {
               console.log(err)
                }
             
           );
 


    this.procesosServ.getEmp()
    .subscribe((res) => {
      if(res.length == 0){
        this.emp=""
      }else{
        this.emp =res
        console.log(this.emp)
          }
        }  , err => {
        console.log(err)
         }
      
    );
  }


  formulario() {
    this.myform = this.a.group({
      empresa: ["", Validators.required]

    });
  }
  onSelectionChanged(e){
   
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    // const selectedData = selectedNodes.map( node => node.data );
    // const selectedDataStringPresentation = selectedData.map( node => node.make + ' ' + node.model).join(', ');
      this.obj = selectedNodes
      if(this.obj.length == 0)
      {
        this.obj = undefined;
       
      }else{
        this.obj =selectedNodes[0].data 
        this.id=this.obj.empCodigo
        this.msjemp=this.obj.empDescripcion
      }
    
  }

showmodal(){
 
  this.confirmarguardar = false
  this.error = false
  this.formulario()
  this.showModal=true
  this.title= "Asociar Empresa ";
 
}

  guardar(){


    let data={
          "grupoId": this.procesosServ.empgrupo,
          "empCodigo":this.myform.value.empresa.empCodigo,
          "secCodigo":this.procesosServ.secCod
         }
         this.procesosServ.postgrupoempresa(data).subscribe(res => {

          if (res == res){
             this.procesosServ.GetEmpresasByGrupo()
            .subscribe(res => {
             this.rowData=res
              this.success = true
              this.confirmarguardar = true
             setTimeout(()=>{ 
              this.success=false
              this.showModal=false
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
             this.confirmarguardar = true
           }
           console.log(err);
         })
         


}
showmodale(){
  this.confirmar = false
  this.error = false
  if(this.obj == undefined){ 
    
    this.title ='Atencion';
    this.eliminar=true
    this.default=false
    this.mesaje= "Debe selecionar una empresa"
    this.confirmar=true
   this.showModale=true
  }else{
    if(this.id ==""){
      this.title ='Atencion';
      this.eliminar=true
      this.default=false
      this.mesaje= "Debe selecionar una empresa"
      this.showModale=true
      this.confirmar=true
     
    }else{
     this.default=true
     this.mesaje ="Desea eliminar la empresa "
     this.showModale=true
    }
  }


}
confirmareliminacion(){
  let data={
    "grupoId": this.procesosServ.empgrupo,
    "empCodigo":this.obj.empCodigo,
    "secCodigo":this.procesosServ.secCod
   }

   this.procesosServ.deletegrupoempresa(data).subscribe(res => {
    if (res == res){
       this.procesosServ.GetEmpresasByGrupo()
      .subscribe(res => {
       this.rowData=res
        this.successelim = true
        this.confirmar = true
       setTimeout(()=>{ 
        this.successelim=false
        this.showModale=false
       }, 1500);
      }, err => {
        console.log(err);
      
      });
    }
  
 }), err => {
    console.log(err);
 }
}


}
