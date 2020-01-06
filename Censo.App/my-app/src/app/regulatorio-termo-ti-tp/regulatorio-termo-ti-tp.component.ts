import { Component, OnInit } from '@angular/core';
import { ExportAsService, ExportAsConfig } from 'ngx-export-as';

/*datepicker*/
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap';
defineLocale('pt-br', ptBrLocale);


@Component({
  selector: 'app-regulatorio-termo-ti-tp',
  templateUrl: './regulatorio-termo-ti-tp.component.html',
  styleUrls: ['./regulatorio-termo-ti-tp.component.css']
})

export class RegulatorioTermoTiTpComponent implements OnInit {
  colorTheme = 'theme-default';

  constructor(private exportAsService: ExportAsService, private localeService: BsLocaleService) {
    localeService.use('pt-br');
  }

  exportAsConfig: ExportAsConfig = {
    type: 'pdf', // the type you want to download
    elementId: 'pdf_View', // the id of html/table element
    options: { // html-docx-js document options
      orientation: 'landscape',
      margins: {     
     top: '0.5',
      left: '0.5',
      bottom: '0.5',
      right: '0.5', 

    },
  }

  };

  export(nome: string) {
    // download the file using old school javascript method
    this.exportAsService.save(this.exportAsConfig, 'MQD - ' + nome).subscribe(() => {
      // save started
    });
    // get the data as base64 or json object for json type - this will be helpful in ionic or SSR
    // this.exportAsService.get(this.config).subscribe(content => {
    //   console.log(content);
    // });
  }



  


  ngOnInit() {

  }

}