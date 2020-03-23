import {Injectable} from '@angular/core';
import {Http,Headers,RequestOptions} from '@angular/http';
import {Router} from '@angular/router';
@Injectable()
export class AuthService{
    constructor(private http:Http,private router:Router){

    }
    baseUrl= "http://localhost:5050/api/auth";
    TOKEN_KEY ="token";
    NAME_KEY="name";

    get isAuthenticated(){
        return !!localStorage.getItem(this.TOKEN_KEY);
    }
    get name(){
        return !!localStorage.getItem(this.NAME_KEY);
    }
    get tokenHeader() {
        var header = new Headers({'Authorization': 'Bearer ' +
         localStorage.getItem(this.TOKEN_KEY)});
         return new RequestOptions({headers: header})
    }
    logout(){
        localStorage.removeItem(this.NAME_KEY);
        localStorage.removeItem(this.TOKEN_KEY);
    }
    login(loginData:any){
        this.http.post(this.baseUrl + "/login",loginData)
            .subscribe(res=>{
                var authResponse = res.json();
                if(!authResponse.result.token){
                    return;
                }
                localStorage.setItem(this.TOKEN_KEY,authResponse.result.token);
                localStorage.setItem(this.NAME_KEY,authResponse.result.userName);
                console.log(localStorage.getItem(this.TOKEN_KEY));
                this.router.navigate(['/'])
            })
    }
    register(user:any){
        delete user.confirmPassword;
        this.http.post(this.baseUrl+ '/register',user)
            .subscribe(res=>{
                var authResponse=res.json();
                if(!authResponse.result.token){
                    return;
                }
                localStorage.setItem(this.TOKEN_KEY,authResponse.result.token);
                localStorage.setItem(this.NAME_KEY,authResponse.result.userName);
                this.router.navigate(['/']);    
            })
    }
}