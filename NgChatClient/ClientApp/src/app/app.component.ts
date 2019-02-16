import { Component } from '@angular/core';
import { ChatAdapter } from 'ng-chat';
import { DemoAdapter } from './demo-adapter';
import { SignalRAdapter } from './signalr-adapter';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private http: HttpClient) { }

  title = 'app';
  currentTheme = 'dark-theme';
  triggeredEvents = [];
  fileUploadUrl: string = `${SignalRAdapter.serverBaseUrl}UploadFile`;

  userId: string = "offline-demo";
  username: string;

  adapter: ChatAdapter = new DemoAdapter();

  switchTheme(theme: string): void {
    this.currentTheme = theme;
  }

  onEventTriggered(event: string): void {
    this.triggeredEvents.push(event);
  }

  joinSignalRChatRoom(): void {
    const userName = prompt('Please enter a user name:');

    this.adapter = new SignalRAdapter(userName, this.http);
  }
}
