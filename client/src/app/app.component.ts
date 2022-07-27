import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/users';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users: any;

  constructor(private accountService: AccountService) { }

  // On Initialization
  ngOnInit() {
    // this.getUsers();
    this.setCurrentUser();
  }

  // to get the user from local storage and set it as current user so that login persists
  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user')); //convert string to object model
    this.accountService.setCurrentUser(user);
  }


  // getUsers() {
  //   //http to get call API methods
  //   //subscribe to get the data after calling the method (called observerables in angular)
  //   this.http.get('https://localhost:5001/api/users').subscribe({
  //     next: response => this.users = response,
  //     error: error => console.log(error)
  //   });
  // }
}
