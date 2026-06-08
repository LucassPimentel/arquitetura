# Configuração dos Serviços de Notificação

## 📧 Email - Mailtrap

### Passos para Configurar:

1. **Criar conta no Mailtrap**
   - Acesse: https://mailtrap.io
   - Clique em "Start Testing for Free"
   - Crie uma nova conta

2. **Obter credenciais SMTP**
   - Na dashboard, vá para "Email Testing"
   - Selecione sua Inbox
   - Clique em "Integrations" → "SMTP"
   - Copie as credenciais:
     - Host: `live.smtp.mailtrap.io`
     - Port: `587`
     - Username: (seu username)
     - Password: (seu password)

3. **Atualizar appsettings.json**
   ```json
   "Mailtrap": {
     "SmtpHost": "live.smtp.mailtrap.io",
     "SmtpPort": 587,
     "Username": "seu_username",
     "Password": "seu_password",
     "FromEmail": "seu-email@mailtrap.io",
     "FromName": "Notification System"
   }
   ```

4. **Testar**
   - Na aplicação, selecione o canal "Email"
   - Digite um email destinatário
   - Envie uma mensagem
   - Verifique em https://mailtrap.io/inbox

### Vantagens:
- ✅ Totalmente gratuito
- ✅ Sem necessidade de cartão de crédito
- ✅ Inbox virtual para testes
- ✅ 500 emails/mês

---

## 💬 WhatsApp - Meta Cloud API

### Passos para Configurar (Sandbox):

1. **Criar App no Facebook Developer**
   - Acesse: https://developers.facebook.com
   - Clique em "Meus Apps" → "Criar App"
   - Escolha "Tipo de App: Business"
   - Defina um nome (ex: "Notification System")

2. **Adicionar Produto WhatsApp**
   - No Dashboard do App, clique em "+ Adicionar Produto"
   - Procure por "WhatsApp"
   - Clique em "Configurar"

3. **Configurar Número de Teste**
   - Vá para "WhatsApp" → "Bem-vindo"
   - Na seção "Teste de API", clique em "Começar"
   - Siga as instruções para adicionar seu número de telefone
   - Confirme o código que receberá via WhatsApp

4. **Obter Credenciais**
   - Na seção "API Setup":
     - Copie o **Temporary Access Token** (válido por 24h)
     - Copie o **Phone Number ID**
   
   Para token permanente:
   - Vá para "Ferramentas" → "Gerenciador de Token"
   - Crie um token de vida útil longa

5. **Atualizar appsettings.json**
   ```json
   "WhatsApp": {
     "AccessToken": "seu_access_token",
     "PhoneNumberId": "seu_phone_number_id",
     "ApiVersion": "v18.0",
     "BaseUrl": "https://graph.instagram.com"
   }
   ```

6. **Testar**
   - Na aplicação, selecione o canal "WhatsApp"
   - Digite o número (ex: +55 11 99999-9999)
   - Envie uma mensagem
   - Você receberá a mensagem no WhatsApp

### Vantagens:
- ✅ Totalmente gratuito no Sandbox
- ✅ Sem limites de mensagens
- ✅ Testar com seu próprio número
- ✅ API oficial do Meta

### Limitações do Sandbox:
- Apenas números adicionados na configuração podem receber mensagens
- Para produção, será necessário submeter para aprovação

---

## 📱 SMS - Simulado

Para SMS em produção, considere:
- **AWS SNS**: 100 SMS/mês grátis (1 ano)
- **Vonage**: Oferece trial
- **MessageBird**: Trial gratuito

Atualmente, o SMS está simulado e apenas registra em logs.

---

## 🧪 Testando Tudo

### 1. Email via Mailtrap
```
Canal: Email
Destinatário: seu-email@mailtrap.io
Assunto: Teste de Notificação
Mensagem: Olá, teste de email!
```

### 2. WhatsApp
```
Canal: WhatsApp
Destinatário: +55 11 99999-9999 (seu número)
Assunto: (pode deixar em branco)
Mensagem: Olá, teste de WhatsApp!
```

### 3. SMS
```
Canal: SMS
Destinatário: +55 11 99999-9999
Assunto: (pode deixar em branco)
Mensagem: Olá, teste de SMS!
```

---

## 📝 Notas Importantes

1. **Segurança**: Nunca commitar credenciais no Git. Use variáveis de ambiente em produção:
   ```csharp
   builder.Configuration["Mailtrap:Username"] = Environment.GetEnvironmentVariable("MAILTRAP_USERNAME");
   ```

2. **Tratamento de Erros**: Os Gateways implementam:
   - Validação de entrada
   - Logging de erros
   - Status de notificação (Pending, Sent, Failed, Retry)

3. **Escalabilidade**: 
   - Adicione mais gateways criando novas classes que implementam `INotificacaoGateway`
   - O `ChannelService` descobrirá automaticamente

---

## 🔗 Links Úteis

- [Mailtrap Documentation](https://docs.mailtrap.io/)
- [Meta WhatsApp Cloud API](https://developers.facebook.com/docs/whatsapp/cloud-api)
- [Notification System Architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture)
