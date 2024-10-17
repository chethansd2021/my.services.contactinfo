import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

// Bootstrap the AppModule to start the application
platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
