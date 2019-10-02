import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OtimizacaoService {

constructor(private http: HttpClient) { }

private baseURL = 'http://10.200.0.9/api/v1/censo/CursoEmec/Otimizar';

Otimizar(obj: any) {

  return this.http.post(this.baseURL, obj);

}


}
