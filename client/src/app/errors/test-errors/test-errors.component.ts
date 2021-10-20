import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
  baseUrl: string = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  getNotFoundError()
  {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe(
      response => {
        console.log(response);
      },
      errors => {
        console.log(errors);
      }
    );
  }

  getBadRequestError()
  {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe(
      response => {
        console.log(response);
      },
      errors => {
        console.log(errors);
      }
    );
  }

  getServerError()
  {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe(
      response => {
        console.log(response);
      },
      errors => {
        console.log(errors);
      }
    );
  }

  getAuthError()
  {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe(
      response => {
        console.log(response);
      },
      errors => {
        console.log(errors);
      }
    );
  }

  get400ValidationError()
  {
    this.http.post(this.baseUrl + 'account/register/', {}).subscribe(
      response => {
        console.log(response);
      },
      errors => {
        console.log(errors);
      }
    );
  }

}
