import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // recieving parent to child parameter
  // @Input() usersFromHomeComponent: any;
  // sending child to parent parameter
  @Output() cancelRegister = new EventEmitter();

  model: any = {};
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }
  //when registered, logs in automatically
  register() {
    this.accountService.register(this.model).subscribe(response => {
      this.cancel();
    }, error => {
      console.log(error);
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
