export type JourneyStep = { title: string; detail: string; done: boolean };
export type ProductConfig = { name:string; eyebrow:string; title:string; subtitle:string; accent:string; accentSoft:string; metric:string; metricLabel:string; steps:JourneyStep[]; cardTitle:string; cardDetail:string; badge:string; action:string; tabs:string[] };
export const product: ProductConfig = {
  name:'EquipCare', eyebrow:'MANUTENÇÃO EM CAMPO', title:'Cada equipamento conta sua história.', subtitle:'Gerador GX-204 · Unidade Central', accent:'#D97706', accentSoft:'#FEEDD0', metric:'92%', metricLabel:'de disponibilidade no mês',
  steps:[{title:'Inspeção preventiva',detail:'Concluída em 14 de outubro',done:true},{title:'Troca de filtro',detail:'Registrada por Carlos M.',done:true},{title:'Revisão elétrica',detail:'Vence em 6 dias',done:false}],
  cardTitle:'Ordem de serviço',cardDetail:'Checklist e fotos prontos para registrar',badge:'PRÓXIMA',action:'Iniciar manutenção',tabs:['Ativos','Agenda','Histórico','Perfil']
};
