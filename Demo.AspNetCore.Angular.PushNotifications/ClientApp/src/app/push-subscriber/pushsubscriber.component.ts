import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SwPush } from '@angular/service-worker';

@Component({
  selector: 'app-push-subscriber',
  templateUrl: './pushsubscriber.component.html',
  styleUrls: ['./pushsubscriber.component.css']
})

export class PushSubscriberComponent {
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  private _subscription: PushSubscription;

  public operationName: string;

  constructor(
    private swPush: SwPush,
    private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
      swPush.subscription.subscribe((subscription) => {
        this._subscription = subscription;
        this.operationName = (this._subscription === null) ? 'Subscribe' : 'Unsubscribe';
      });    
  };

  operation() {
    (this._subscription === null) ? this.subscribe() : this.unsubscribe(this._subscription.endpoint);
  };

  private subscribe() {
    this.httpClient.get(this.baseUrl + 'api/PublicKey', { responseType: 'text' }).subscribe(publicKey => {
      this.swPush.requestSubscription({
        serverPublicKey: publicKey
      })
        .then(subscription => this.httpClient.post(this.baseUrl + 'api/PushSubscriptions', subscription, this.httpOptions).subscribe(() => { }, error => console.error(error)))
        .catch(error => console.error(error));
    }, error => console.error(error));
  };

  private unsubscribe(endpoint) {
    this.swPush.unsubscribe()
      .then(() => this.httpClient.delete(this.baseUrl + 'api/PushSubscriptions/' + encodeURIComponent(endpoint)).subscribe(() => { }, error => console.error(error)))
      .catch(error => console.error(error));
  }
}
