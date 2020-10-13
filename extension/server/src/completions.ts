import {
    CompletionItemKind,
} from 'vscode-languageserver';

const completions = [
    {
        label: 'return',
        kind: CompletionItemKind.Method,
        detail: 'return method',
        documentation: 'return SomeVariable;',
        data: 1
    },
];
export default completions;