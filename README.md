--------- LEIA -------------------

<Resumo>
Projeto capaz de ler um excel contendo o apontamento de horas dos funcionários, num determinado mês, e gerar, a partir disto, um arquivo json contendo informações como: salário, horas extras e outras métricas.

Framework: .NET Core 3.1
Template: Console App.

<Importante>
O arquivo .csv originalmente enviado não estava com o Encoding adequado, de modo que os carácteres especiais não eram reconhecidos ao ler o arquivo. O arquivo foi salvo novamente, sem alteração de conteúdo, utilizando o padrão CSV UTF-8 (Delimitado por vírgulas).

Para melhoria no desempenho, além de tornar os métodos assíncronos, foi adotado um modelo em que o usuário delimita, de antemão, o mês de vigência para geração do documento Balanco_Mes.

Foi adotado o template Console App devido a simplicidade do projeto, que não requeria uma interface de usuário mais detalhada para realização da operação, tratando-se mais de uma automação.

<Por fim>
Agradecimentos à equipe Auvo Sistemas.
