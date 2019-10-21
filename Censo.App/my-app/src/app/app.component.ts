import { Component, Output } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'Inicio';

  HideToast() {
    document.getElementById('ttoast').classList.replace('show', 'hide');
  }

  ShowToast() {
    document.getElementById('ttoast').classList.replace('hide', 'show');
    // this.myEvent.emit(null);
  }

  fechar(){
    this.HideToast();
  }

}
