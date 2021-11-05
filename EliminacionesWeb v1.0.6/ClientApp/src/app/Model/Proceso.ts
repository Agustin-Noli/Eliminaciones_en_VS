export interface IProceso{

  empCodigo: number,
  empDescripcion: string,
  periodo:string, //072020
  rubCodigo: string,
  rubDescripcion: string,
  concepto: string,
  empCodigoContraparte: number,
  empDescripcionContraparte: string,
  monCodigo: number, //nulleable ?
  monDescripcion: string,
  saldo: number, //nulleable ?
  saldoPromedio: number, //nulleable ?
  ind: number, //nulleable ?
  pond: number, //nulleable ?
  exposicion: string,
  secCodigo:number,
  secdescripcion: string
  }


    // "empCodigo": 1,
  // "empDescripcion": "B.G.B.A.",
  // "periodo": "202007",
  // "rubCodigo": "0000111015",
  // "rubDescripcion": "B.C.R.A.-CUENTA CORRIENTE               ",
  // "concepto": "test concepto",
  // "empCodigoContraparte": 3,
  // "empDescripcionContraparte": "Tarjetas Cuyanas S.A. (interco)",
  // "monCodigo": 0,
  // "monDescripcion": "PESOS",
  // "saldo": 7000.00,
  // "saldoPromedio": 10000.00,
  // "ind": null,
  // "pond": null,
  // "exposicion": null