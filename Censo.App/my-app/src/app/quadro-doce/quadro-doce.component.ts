import { Component, OnInit } from '@angular/core';
import { ExportAsService, ExportAsConfig } from 'ngx-export-as';

@Component({
  selector: 'app-quadro-doce',
  templateUrl: './quadro-doce.component.html',
  styleUrls: ['./quadro-doce.component.css']
})
export class QuadroDoceComponent implements OnInit {

  constructor(private exportAsService: ExportAsService) { }

  exportAsConfig: ExportAsConfig = {
    type: 'pdf', // the type you want to download
    elementId: 'pdf_View', // the id of html/table element

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
